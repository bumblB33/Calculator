
using System.Linq.Expressions;

namespace Calculator;

public class MathOperations
{
    private readonly List<decimal> numerals = new();
    public MathOperations(List<decimal> numerals)
    {
        this.numerals = numerals;

    }
    public string PerformOperation(string operation)
    {
        string result = "";
        switch (operation)
        {
            case "Addition":
                return AdditionOperation(numerals);
            case "Subtraction":
                return SubtractionOperation(numerals);
            case "Division":
                return DivisionOperation(numerals);
            case "Multiplication":
                return MultiplicationOperation(numerals);
            case "":
                return string.Empty;

        }
        return result;
    }
    internal static string AdditionOperation(List<decimal> numerals)
    {
        if (numerals.Count == 0)
        {
            return "";
        }
        decimal sum = numerals.Sum();
        string result = $"{string.Join(" + ", numerals)} = {sum}";

        return result;
    }
    internal static string SubtractionOperation(List<decimal> numerals)
    {
        if (numerals.Count == 0)
        {
            return "";
        }
        decimal difference = numerals[0];
        foreach (decimal d in numerals[1..])
        {
            difference -= d;
        }
        string result = $"{numerals[0]} - {string.Join(" - ", numerals[1..])} = {difference}";
        return result;
    }

    internal static string MultiplicationOperation(List<decimal> numerals)
    {
        if (numerals.Count == 0)
        {
            return "";
        }
        var product = numerals[0];
        foreach (decimal d in numerals[1..])
        {
            product *= d;
        }
        string result = $"{numerals[0]} * {string.Join(" * ", numerals[1..])} = {product}";
        return result;
    }

    internal string DivisionOperation(List<decimal> numerals)
    {
        try
        {
            var zero_values = numerals[1..].FindAll(x => x == 0);
            if (zero_values.Count > 0)
            {
                throw new DivideByZeroException("Cannot divide by zero. Please try again.\n");
            }

            var operation_result = numerals[0];
            foreach (decimal d in numerals[1..])
            {
                operation_result /= d;
            }
            string result = $"{numerals[0]} / {string.Join(", ", numerals[1..])} = {operation_result}";
            return result;
        }
        catch (DivideByZeroException ex)
        {
            return ex.Message;
        }
    }


}
