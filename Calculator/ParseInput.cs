using System.Text.RegularExpressions;

namespace Calculator;

public class ProcessInputOptions
{
    public List<string> Delimiters { get; set; } = [","];

    // Whether we set a limit on the quantity of numbers that are accepted. Null means there is no limit.
    public int? LimitInputQuantity { get; set; }

    // Maximum value allowed for each numeral input. Null means no maximum value.
    public decimal? MaximumValue { get; set; }

    public bool DenyNegativeValues { get; set; }

    // Whether a single character custom delimiter can be defined with the format (//{delimiter}\n).
    public bool AllowSingleCharCustomDelimiter { get; set; }

    // Whether one multi-character custom delimiter can be defined with the format (//{delimiter}\n).
    public bool AllowMultiCharCustomDelimiter { get; set; }

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

    public ProcessResult Process(string? input)
    {

        _Delimiters = options.Delimiters;

        if (_inputText == null)
        {
            _inputText = ReadInput();
            var numerals = ParseInputToNumerals();
            return ApplyOptions(numerals);
        }
        else
        {
            _inputText = ReadInput(input);
            var numerals = ParseInputToNumerals();
            return ApplyOptions(numerals);
        }

    }

    public string GetInputText()
    {
        return _inputText;
    }

    private string ReadInput(string? _inputText = null)
    {
        {
            if (_inputText == null)
            {
                _inputText = Console.ReadLine() ?? string.Empty;
            }

            if (string.IsNullOrEmpty(_inputText))
            {
                _inputText = "0";

            }
            _inputText = _inputText.Replace("\n", "\\n");
            if (options.AllowSingleCharCustomDelimiter == true)
            {
                _inputText = ProcessSingleCharCustomDelimiter(string.IsNullOrEmpty(_inputText) ? "0" : _inputText);
            }

            if (options.AllowMultiCharCustomDelimiter == true)
            {
                _inputText = ProcessMultiCharCustomDelimiter(string.IsNullOrEmpty(_inputText) ? "0" : _inputText);
            }

            if (options.AllowMultipleCustomDelimiters == true)
            {
                _inputText = ProcessMultipleCustomDelimiters(_inputText: _inputText);
            }
            return _inputText;
        }
    }

    private string ProcessSingleCharCustomDelimiter(string _inputText)
    {
        var match = Regex.Match(_inputText, pattern: @"^//(.)\\n(.*)$");
        if (match.Success)
        {
            Console.WriteLine($"Custom delimiter found: {match.Groups[1].Value}\n");
            _Delimiters.Add(match.Groups[1].Value);

            _inputText = match.Groups[2].Value;
            Console.WriteLine($"Removed custom delimiter definition from the input text: {_inputText}\n");
            return _inputText;
        }
        else
        {
            Console.WriteLine("No custom delimiter found.\n");
            Console.WriteLine("Was your custom delimiter longer than a single character?\n");
            Console.WriteLine("If so, please try again.\n");
            return _inputText;
        }
    }

    private string ProcessMultiCharCustomDelimiter(string _inputText)
    {
        var match = Regex.Match(_inputText, pattern: @"^//\[(.*)\]\\n(.*)$");
        if (match.Success)
        {
            Console.WriteLine($"Custom delimiter found: {match.Groups[1].Value}\n");
            _Delimiters.Add(match.Groups[1].Value);

            _inputText = match.Groups[2].Value;
            Console.WriteLine($"Removed custom delimiter definition from the input text: {_inputText}\n");
            return _inputText;
        }
        else
        {
            Console.WriteLine("No custom delimiter found.\n");
            Console.WriteLine("Did you try to define multiple custom delimiters?\n");
            Console.WriteLine("If so, please try again.\n");
            return _inputText;
        }
    }
    private string ProcessMultipleCustomDelimiters(string _inputText)
    {
        var match = Regex.Match(_inputText, @"^//(?:\[([^\]]+)\])+\\n(.*)$", RegexOptions.Singleline);
        if (match.Success)
        {
            var delimiterMatches = Regex.Matches(_inputText, @"\[([^\]]+)\]");

            foreach (Match delimiterMatch in delimiterMatches)
            {
                if (delimiterMatch.Success)
                {
                    _Delimiters.Add(delimiterMatch.Groups[1].Value);
                }
            }

            Console.WriteLine($"Delimiters accepted:\n");
            for (int i = 0; i < _Delimiters.Count; i++)
            {
                Console.WriteLine($"Delimiter {i + 1}: {_Delimiters[i]}");
            }

            _inputText = match.Groups[2].Value;
            Console.WriteLine($"Removed custom delimiter definitions from the input text: {_inputText}\n");
            return _inputText;
        }
        else
        {
            Console.WriteLine("No custom delimiters found.\n");
            return _inputText;
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
        if (numerals.Count == 1)
        {
            numerals.Add(InvalidInputReplacement);
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
                result.ErrorMessage = $"Negative values are not allowed. You entered: {string.Join(", ", negatives)}\n\n";
                result.Values = new List<decimal>();
                return result;
            }
        }

        if (options.LimitInputQuantity.HasValue && numerals.Count != options.LimitInputQuantity.Value)
        {
            result.Success = false;
            result.ErrorMessage = $"Expected {options.LimitInputQuantity.Value} numbers, but got {numerals.Count}.\n" +
                                  $"Please provide exactly {options.LimitInputQuantity.Value} numbers.\n";
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
                    Console.WriteLine($"The maximum value for number inputs is {options.MaximumValue.Value}.\n" +
                                          $"The number you entered that exceeded the limit was {numerals[i]}.\n" +
                                          "It was replaced with a zero.\n");

                    numerals[i] = 0;

                }
            }
        }

        result.Values = numerals;
        return result;
    }


}