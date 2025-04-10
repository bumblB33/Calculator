using System.Text.RegularExpressions;

namespace Challenge;

public class ProcessInputOptions
{
    public List<string> Delimiters { get; set; } = [","];

    // Whether we set a limit on the quantity of numbers that are accepted. Null means there is no limit.
    public int? LimitInputQuantity { get; set; }

    // Maximum value allowed for each numeral input. Null means no maximum value.
    public decimal? MaximumValue { get; set; }

    public bool DenyNegativeValues { get; set; }

    // Whether a single custom delimiter can be defined with the format (//{delimiter}\n).
    public bool AllowSingleCustomDelimiter { get; set; }

    // Whether multiple custom delimiters can be defined with the format (//[{delimiter}][{delimiter}]...\n).
    public bool AllowMultipleCustomDelimiters { get; set; }
}

public class ProcessResult
{
    public List<decimal> Values { get; set; } = [];
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

public class ProcessInput
{
    private const decimal InvalidInputReplacement = 0;
    required public ProcessInputOptions options;
    private string _inputText = string.Empty;
    private List<string> _Delimiters = [];

    // Constructs the default instance.
    public ProcessInput()
        : this(new ProcessInputOptions())
    {
    }

    public ProcessInput(ProcessInputOptions options)
    {
        options = options ?? new ProcessInputOptions();
        _Delimiters = options.Delimiters;

    }

    public ProcessResult Process()
    {
        _Delimiters = options.Delimiters;

        ReadInput();

        var numerals = ParseInputToNumerals();

        return ApplyOptions(numerals);
    }

    public string GetInputText()
    {
        return _inputText;
    }

    private void ReadInput()
    {
        _inputText = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrEmpty(_inputText))
        {
            _inputText = "0";
            return;
        }

        _inputText = _inputText.Replace("\\n", "\n");

        if (options.AllowSingleCustomDelimiter)
        {
            ProcessSingleCustomDelimiter();
        }

        if (options.AllowMultipleCustomDelimiters)
        {
            ProcessMultipleCustomDelimiters();
        }
    }

    private void ProcessSingleCustomDelimiter()
    {
        var match = Regex.Match(_inputText, @"^//(.)\n(.*)$", RegexOptions.Singleline);
        if (match.Success)
        {
            var delimiter = match.Groups[1].Value;
            Console.WriteLine($"Custom delimiter found: {delimiter}");
            _Delimiters.Add(delimiter);


            _inputText = match.Groups[2].Value;
            Console.WriteLine($"Removed custom delimiter definition from the input text: {_inputText}");
        }
    }
    private void ProcessMultipleCustomDelimiters()
    {
        var match = Regex.Match(_inputText, @"^//(?:\[([^\]]+)\])+\n(.*)$", RegexOptions.Singleline);
        if (match.Success)
        {
            var delimiterMatches = Regex.Matches(_inputText, @"\[([^\]]+)\]");
            var customDelimiters = delimiterMatches
                .Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .ToList();

            Console.WriteLine($"Custom delimiters found: {string.Join(", ", customDelimiters)}");
            _Delimiters.AddRange(customDelimiters);

            _inputText = match.Groups[2].Value;
            Console.WriteLine($"Removed custom delimiter definitions from the input text: {_inputText}");
        }
    }

    private List<decimal> ParseInputToNumerals()
    {
        var numerals = new List<decimal>();

        var inputs = _inputText.Split(_Delimiters.ToArray(), StringSplitOptions.None);

        foreach (var input in inputs)
        {
            if (decimal.TryParse(input, out decimal value))
            {
                numerals.Add(value);
            }
            else if (!string.IsNullOrWhiteSpace(input))
            {
                // Add replacement values if the string is empty
                numerals.Add(InvalidInputReplacement);
            }
        }

        // If there is only one string
        if (numerals.Count == 0 && !string.IsNullOrWhiteSpace(_inputText))
        {
            if (decimal.TryParse(_inputText, out decimal value))
            {
                numerals.Add(value);
            }
        }

        return numerals;
    }

    private ProcessResult ApplyOptions(List<decimal> numerals)
    {
        var result = new ProcessResult { Values = numerals, Success = true };

        if (options.DenyNegativeValues)
        {
            var negatives = numerals.Where(n => n < 0).ToList();
            if (negatives.Any())
            {
                result.Success = false;
                result.ErrorMessage = $"Negative values are not allowed: {string.Join(", ", negatives)}";
                result.Values = new List<decimal>();
                return result;
            }
        }

        if (options.LimitInputQuantity.HasValue && numerals.Count != options.LimitInputQuantity.Value)
        {
            result.Success = false;
            result.ErrorMessage = $"Expected {options.LimitInputQuantity.Value} numbers, but got {numerals.Count}.";
            result.Values = new List<decimal>();
            return result;
        }

        // Replace values exceeding maximum with zero
        if (options.MaximumValue.HasValue)
        {
            for (int i = 0; i < numerals.Count; i++)
            {
                if (numerals[i] > options.MaximumValue.Value)
                {
                    numerals[i] = 0;
                }
            }
        }

        result.Values = numerals;
        return result;
    }


}