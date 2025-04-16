// 1.Support a maximum of 2 numbers using a comma delimiter.Throw an exception when more than 2 numbers are provided
// 	* examples: `20` will return `20`; `1,5000` will return `5001`; `4,-3` will return `1`
// 	* empty input or missing numbers should be converted to `0`
// 	* invalid numbers should be converted to `0` e.g. `5, tytyt` will return `5`

using System.Management;

namespace Calculator;
public class Program
{
    readonly ProcessInputOptions options = new ProcessInputOptions();
    Introduction Introduction { get; set; } = new Introduction() { options = new ProcessInputOptions() };
    ProcessInput Input { get; set; } = new ProcessInput() { options = new ProcessInputOptions() };
    public Program()
    {
    }
    public Program(ProcessInputOptions options) : this()
    {
        this.options = options;

        Introduction introduction = new Introduction()
        {
            options = options,
        };
        this.Introduction = introduction;

        ProcessInput input = new ProcessInput()
        {
            options = options
        };
        this.Input = input;
    }



    public static void ExitHandler(object? sender, ConsoleCancelEventArgs args)
    {
        Console.WriteLine("The calculator program will now exit.\n");
        args.Cancel = false;
    }
    public string Calculate(string? inputs = null, string? _operation = null)
    {
        Introduction.DescribeConfiguration();
        List<decimal> Numerals;
        ProcessResult Result = Input.Process(input: inputs);
        if (Result.Success == false)
        {
            return Result.ErrorMessage;
        }
        Numerals = Result.Values;
        string operationResult = GetResult(_operation, Numerals);
        return operationResult;
    }

    private string GetResult(string? _operation, List<decimal> Numerals)
    {
        string operationResult;
        if (_operation == null)
        {
            operationResult = Introduction.SelectOperation(Numerals);
        }
        else
        {
            operationResult = Introduction.SelectOperation(Numerals, _operation);
        }

        return operationResult;
    }

    public static void Main()
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler(ExitHandler);
        while (true)
        {
            ProcessInputOptions options = new ProcessInputOptions()
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
                MaximumValue = 1000,
                AllowSingleCustomDelimiter = true,
            };
            Program Calculator = new Program(options: options);
            string operationResult = Calculator.Calculate();
            Console.WriteLine(operationResult); // Ensure this line is executed
            Console.WriteLine("Press Ctrl+C to exit the calculator program.");
            continue;
        }
    }
}
