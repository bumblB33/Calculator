using NUnit.Framework;
using Shouldly;

namespace Calculator.Tests
{
    public class ProgramTest
    {
        [Test]
        public void TestAdditionOperation()
        {
            var options = new ProcessInputOptions
            {
                LimitInputQuantity = 2,
            };

            var program = new Program(options);
            string result = program.Calculate("1,2", "1");
            result.ShouldBe("1 + 2 = 3");
        }
        [Test]
        public void TestSubtractionOperation()
        {
            var options = new ProcessInputOptions
            {
                LimitInputQuantity = 2,
            };

            var program = new Program(options);
            string result = program.Calculate("10,2", "2");
            result.ShouldBe("10 - 2 = 8");
        }
        [Test]
        public void TestMultiplicationOperation()
        {
            var options = new ProcessInputOptions
            {
                LimitInputQuantity = 2,
            };

            var program = new Program(options);
            string result = program.Calculate("2,3", "3");
            result.ShouldBe("2 * 3 = 6");
        }
        [Test]
        public void TestDivisionOperation()
        {
            var options = new ProcessInputOptions
            {
                LimitInputQuantity = 2,
            };

            var program = new Program(options);
            string result = program.Calculate("20,4", "4");
            result.ShouldBe("20 / 4 = 5");
        }
        [Test]
        public void TestNumberQuantityExceeded()
        {
            var options = new ProcessInputOptions
            {
                LimitInputQuantity = 2,
            };

            var program = new Program(options);
            string result = program.Calculate("1,2,3", "InvalidOperation");
            result.ShouldBe("Expected 2 numbers, but got 3.\n" +
                            $"Please provide exactly 2 numbers.\n");
        }
        [Test]
        public void TestInvalidInput()
        {
            var options = new ProcessInputOptions
            {
                LimitInputQuantity = 2,
            };

            var program = new Program(options);
            string result = program.Calculate("5,tytyt", "1");
            result.ShouldBe("5 + 0 = 5");
        }
        [Test]
        public void TestNoLimitNumberQuantity()
        {
            var options = new ProcessInputOptions
            {
                LimitInputQuantity = null,
            };

            var program = new Program(options);
            string result = program.Calculate("1,2,3,4,5,6,7,8,9,10,11,12", "1");
            result.ShouldBe("1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 = 78");
        }
        [Test]
        public void TestNewLineDelimiter()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" }
            };

            var program = new Program(options);
            string result = program.Calculate("1\n2,3", "1");
            result.ShouldBe("1 + 2 + 3 = 6");
        }
        [Test]
        public void TestNegativeNumberException()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
            };

            var program = new Program(options);
            string result = program.Calculate("1,-2,3", "1");
            result.ShouldBe("Negative values are not allowed. You entered: -2\n\n");
        }
        [Test]
        public void TestEmptyInput()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
            };

            var program = new Program(options);
            string result = program.Calculate("", "1");
            result.ShouldBe("0 + 0 = 0");
        }
        [Test]
        public void TestMaximumValueExceeded()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
                MaximumValue = 1000,
            };

            var program = new Program(options);
            string result = program.Calculate("1,2000,3", "1");
            result.ShouldBe("1 + 0 + 3 = 4");
        }
        [Test]
        public void TestDivideByZeroException()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
            };

            var program = new Program(options);
            string result = program.Calculate("1,0", "4");
            result.ShouldBe("Cannot divide by zero. Please try again.\n");
        }
        [Test]
        public void TestSingleCharCustomDelimiter()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
                AllowSingleCharCustomDelimiter = true,
            };

            var program = new Program(options);
            string result = program.Calculate("//#\n2#5", "1");
            result.ShouldBe("2 + 5 = 7");
        }
        [Test]
        public void TestAlternateSingleCharCustomDelimiter()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
                AllowSingleCharCustomDelimiter = true,
            };

            var program = new Program(options);
            string result = program.Calculate("//,\n2,ff,100", "1");
            result.ShouldBe("2 + 0 + 100 = 102");

        }
        [Test]
        public void TestInvalidCustomDelimiters()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
                AllowSingleCharCustomDelimiter = true,
            };

            var program = new Program(options);
            string result = program.Calculate("//[;][#]\n1,2#3;4", "1");
            // String is split on the accepted delimiters \n and ,
            // Which gives three groups: 
            //      1. //[;][#]
            //      2. 1
            //      3. 2#3;4
            // The first and last group cannot be parsed as numbers. 
            // So, they are treated as invalid inputs and replaced by zeroes.
            result.ShouldBe("0 + 1 + 0 = 1");
        }
        [Test]
        public void TestMultiCharCustomDelimiter()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
                AllowMultiCharCustomDelimiter = true,
            };

            var program = new Program(options);
            string result = program.Calculate("//[***]\n1***2,100", "1");
            result.ShouldBe("1 + 2 + 100 = 103");
        }
        [Test]
        public void TestMultiCharCustomDelimiterUsage()
        {
            var options = new ProcessInputOptions
            {
                Delimiters = new List<string> { ",", "\\n" },
                DenyNegativeValues = true,
                AllowMultiCharCustomDelimiter = true,
            };

            var program = new Program(options);
            string result = program.Calculate("//[***][#]\n1***2#3", "1");
            result.ShouldBe("0 + 0 = 0");
        }
    }
}