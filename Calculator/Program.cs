// 7.Support 1 custom delimiter of any length using the format: `//[{delimiter}]\n{numbers}`
// 	*example: `//[***]\n11***22***33` will return `66`
// 	*all previous formats should also be supported

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
                AllowMultiCharCustomDelimiter = true,
            };
            Program Calculator = new Program(options: options);
            string operationResult = Calculator.Calculate();
            Console.WriteLine(operationResult); // Ensure this line is executed
            Console.WriteLine("Press Ctrl+C to exit the calculator program.");
            continue;
        }
    }
}
