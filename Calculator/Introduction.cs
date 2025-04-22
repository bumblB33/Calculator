namespace Calculator;
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
    public void DescribeConfiguration()
    {
        {
            Console.WriteLine("Welcome to the calculator! Please enter your numbers, separating them with delimiters.");
            Console.WriteLine($"Delimiters accepted:\n");
            for (int i = 0; i < options.Delimiters.Count; i++)
            {
                Console.WriteLine($"Delimiter {i + 1}: {options.Delimiters[i]}");
            }
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

            if (options.AllowSingleCharCustomDelimiter)
            {
                Console.WriteLine("You can also define a single character custom delimiter.");
                Console.WriteLine("Add one by prefixing your input using the format //{delimiter}\\n.");
                Console.WriteLine("Example: //#\\n2#5 and the Addition operation will return 7.");
            }
            if (options.AllowMultiCharCustomDelimiter)
            {
                Console.WriteLine("You can also define a multi-character custom delimiter.");
                Console.WriteLine("Add one by prefixing your input using the format //{delimiter}\\n.");
                Console.WriteLine("Example: //[***]\\n1***2,100 and the Addition operation will return 103.");
            }

            if (options.AllowMultipleCustomDelimiters)
            {
                Console.WriteLine("You can define multiple custom delimiters of any length by prefixing your input.");
                Console.WriteLine("Add them using the format //[{delimiter1}][{delimiter2}]...\\n).");
                Console.WriteLine("Example: //[***][#]\\n1***2#100 and the Addition operation will return 103.");

            }

        }
    }

    public string SelectOperation(List<decimal> Numerals, string? selectedOperation = null)
    {
        string v;
        if (string.IsNullOrEmpty(selectedOperation))
        {
            Console.WriteLine("Please select a math operation from the following list:");
            Console.WriteLine("\t1 - Addition");
            Console.WriteLine("\t2 - Subtraction");
            Console.WriteLine("\t3 - Multiplication");
            Console.WriteLine("\t4 - Division");
            Console.Write("Your option:\n");

            v = Console.ReadLine() ?? string.Empty;
        }
        else
        {
            v = selectedOperation ?? string.Empty;
        }
        if (string.IsNullOrEmpty(v))
        {
            return "No valid operation selected. Please try again!";
        }

        MathOperations operation = new MathOperations(numerals: Numerals);
        switch (v)
        {
            case "1" or "Addition":
                return operation.PerformOperation("Addition");
            case "2" or "Subtraction":
                return operation.PerformOperation("Subtraction");
            case "3" or "Multiplication":
                return operation.PerformOperation("Multiplication");
            case "4" or "Division":
                return operation.PerformOperation("Division");
            default:
                return "No valid operation selected. Please try again!";
        }
    }

}