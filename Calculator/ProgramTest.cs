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
    }
}