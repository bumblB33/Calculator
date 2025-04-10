namespace Challenge;
public class Introduction()
{

    required public ProcessInputOptions options { get; set; }


    public Introduction(ProcessInputOptions? inputOptions) : this()
    {
        if (inputOptions == null)
        {
            this.options = new ProcessInputOptions();
        }
        else
        {
            this.options = inputOptions;
        }
    }

    public void SelectOperation(List<decimal> Numerals)
    {
        MathOperations operation = new MathOperations(numerals: Numerals);

        Console.WriteLine("Select a math operation from the following list:");
        Console.WriteLine("\t1 - Addition");
        Console.WriteLine("\t2 - Subtraction");
        Console.WriteLine("\t3 - Multiplication");
        Console.WriteLine("\t4 - Division");
        Console.Write("Your option:\n");

        string? v = Console.ReadLine();
        if (v != null)
        {
            switch (v)
            {
                case "1":
                    Console.WriteLine(operation.PerformOperation("Addition"));
                    break;
                case "2":
                    Console.WriteLine(operation.PerformOperation("Subtraction"));
                    break;
                case "3":
                    Console.WriteLine(operation.PerformOperation("Multiplication"));
                    break;
                case "4":
                    Console.WriteLine(operation.PerformOperation("Division"));
                    break;
                default:
                    Console.WriteLine("No valid operation selected. Please try again!");
                    break;
            }
        }

    }



    public void DescribeConfiguration()
    {
        {
            Console.WriteLine("Welcome to the calculator! Please enter your numbers, separating them with delimiters.");
            Console.WriteLine($"The default delimiters accepted are: {string.Join(" or ", options.Delimiters)}");

            if (options.MaximumValue.HasValue)
            {
                Console.WriteLine($"Set maximum value for numeric input to {options.MaximumValue}.");
            }

            if (options.LimitInputQuantity.HasValue)
            {
                Console.WriteLine($"The maximum amount of numbers you can provide is {options.LimitInputQuantity}.");
            }

            if (options.DenyNegativeValues)
            {
                Console.WriteLine("Negative values are not allowed.");
            }

            if (options.AllowSingleCustomDelimiter)
            {
                Console.WriteLine("You can define a single custom delimiter with the format (//{delimiter}\\n).");
            }

            if (options.AllowMultipleCustomDelimiters)
            {
                Console.WriteLine("You can define multiple custom delimiters with the format (//[{delimiter1}][{delimiter2}]...\\n).");
            }

        }
    }

}