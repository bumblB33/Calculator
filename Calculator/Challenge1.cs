// 1.Support a maximum of 2 numbers using a comma delimiter.Throw an exception when more than 2 numbers are provided
// 	* examples: `20` will return `20`; `1,5000` will return `5001`; `4,-3` will return `1`
// 	* empty input or missing numbers should be converted to `0`
// 	* invalid numbers should be converted to `0` e.g. `5, tytyt` will return `5`

namespace Challenge;
class Challenge1()
{
    readonly ProcessInputOptions options = new ProcessInputOptions();
    Introduction Introduction { get; set; } = new Introduction() { options = new ProcessInputOptions() };
    ProcessInput Input { get; set; } = new ProcessInput() { options = new ProcessInputOptions() };
    public Challenge1(ProcessInputOptions options) : this()
    {
        this.options = options;
        this.options.LimitInputQuantity = 2;

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
    public void Calculate()
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler(ExitHandler);
        while (true)
        {
            Introduction.DescribeConfiguration();
            ProcessResult Result = Input.Process();
            List<decimal> Numerals = Result.Values;
            Introduction.SelectOperation(Numerals);
        }
    }

    public static void Main()
    {
        Challenge1 challenge = new Challenge1();
        challenge.Calculate();
    }
}
