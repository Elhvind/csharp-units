using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Units.Mass;

namespace Units.Tests;

public class PercentageTests
{
    public class StructTests
    {
        [Fact]
        public void ShouldReturnDefaultValueWhenNull()
        {
            Percentage? value = null;
            Percentage expected = new(0);

            Percentage actual = value.GetValueOrDefault();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnFraction()
        {
            var percentage1 = new Percentage(100);
            var percentage2 = new Percentage(50);
            var percentage3 = new Percentage(25);

            percentage1.Fraction.Should().Be(1.00);
            percentage2.Fraction.Should().Be(0.50);
            percentage3.Fraction.Should().Be(0.25);
        }

        [Fact]
        public void ShouldCalculateRatio()
        {
            var amount = 10;
            var totalAmount = 100;
            var expected = new Percentage(10);

            var actual = new Percentage(amount, totalAmount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void OfReturnsTheAmount()
        {
            var percentage = new Percentage(10);
            var amount = 100d;
            var expected = 10d;

            var actual = percentage.Of(amount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void OfReturnsTheIntAmount()
        {
            Percentage percentage = new(10);
            int amount = 100;
            int expected = 10;

            double actual = percentage.Of(amount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void OfReturnsTheFloatAmount()
        {
            Percentage percentage = new(10);
            float amount = 100;
            float expected = 10;

            double actual = percentage.Of(amount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void OfReturnsTheKilogramAmount()
        {
            Percentage percentage = new(10);
            Kilogram amount = new(100);
            Kilogram expected = new(10);

            Kilogram actual = percentage.Of(amount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void OfReturnsTheLiterAmount()
        {
            Percentage percentage = new(10);
            Liter amount = new(100);
            Liter expected = new(10);

            Liter actual = percentage.Of(amount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void RemainderReturnsTheRemainingAmount()
        {
            var percentage = new Percentage(10);
            var amount = 10d;
            var expected = 90d;

            var actual = percentage.Remainder(amount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void TotalReturnsTheTotalAmount()
        {
            var percentage = new Percentage(10);
            var amount = 10d;
            var expected = 100d;

            var actual = percentage.Total(amount);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformAddition()
        {
            var left = new Percentage(10.5);
            var right = new Percentage(10.5);
            var expected = new Percentage(21);

            var actual = left + right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformSubtraction()
        {
            var left = new Percentage(10.5);
            var right = new Percentage(5.5);
            var expected = new Percentage(5);

            var actual = left - right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformMultiplication()
        {
            var left = new Percentage(10.5);
            var right = new Percentage(10);
            var expected = new Percentage(105);

            var actual = left * right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDivision()
        {
            var left = new Percentage(10);
            var right = new Percentage(5);
            var expected = new Percentage(2);

            var actual = left / right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformImplicitConversionToDouble()
        {
            const double expected = 10;
            double actual = new Percentage(10);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformExplicitConversion()
        {
            const double valueDouble = 10d;
            const float valueFloat = 10f;
            const int valueInt = 10;

            var expected = new Percentage(valueDouble);
            var actualD = (Percentage)valueDouble;
            var actualF = (Percentage)valueFloat;
            var actualI = (Percentage)valueInt;

            actualD.Should().Be(expected);
            actualF.Should().Be(expected);
            actualI.Should().Be(expected);
        }

        [Theory]
        [InlineData(10.5, 10.5, true)]
        [InlineData(5.5, 10.5, false)]
        [InlineData(10.5, 5.5, false)]
        public void ShouldBeEqualUsingEquals(double leftDouble, double rightDouble, bool equal)
        {
            Percentage left = new(leftDouble);
            Percentage right = new(rightDouble);

            var actual = left.Equals(right);

            actual.Should().Be(equal);
        }

        [Theory]
        [InlineData(10.5, 10.5, true, false, false, true, false, true)]
        [InlineData(5.5, 10.5, false, true, true, true, false, false)]
        [InlineData(10.5, 5.5, false, true, false, false, true, true)]
        public void ShouldPerformLogicalOperations(double leftDouble, double rightDouble, bool equal, bool notEqual, bool greater, bool greaterOrEqual, bool less, bool lessOrEqual)
        {
            Percentage left = new(leftDouble);
            Percentage right = new(rightDouble);

            (left == right).Should().Be(equal);
            (left != right).Should().Be(notEqual);
            (left < right).Should().Be(greater);
            (left <= right).Should().Be(greaterOrEqual);
            (left > right).Should().Be(less);
            (left >= right).Should().Be(lessOrEqual);
        }
    }

    public class TypeConverterTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(15)]
        public void ConvertFromInteger(int value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Percentage));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Percentage>();
            if (obj is Percentage actual)
                actual.Should().Be(new Percentage(value));
        }

        [Theory]
        [InlineData(0.575)]
        [InlineData(10)]
        [InlineData(10.5)]
        public void ConvertFromDouble(double value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Percentage));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Percentage>();
            if (obj is Percentage actual)
                actual.Should().Be(new Percentage(value));
        }

        [Theory]
        [InlineData("0.575", 0.575)]
        [InlineData("10", 10)]
        [InlineData("10.5", 10.5)]
        public void ConvertFromString(string value, double expected)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Percentage));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Percentage>();
            if (obj is Percentage actual)
                actual.Should().Be(new Percentage(expected));
        }
    }

    public class JsonConverterTests
    {
        [Theory]
        [InlineData(0.575, "0.575")]
        [InlineData(10, "10")]
        [InlineData(10.5, "10.5")]
        [InlineData(double.NegativeInfinity, "\"-Infinity\"")]
        [InlineData(double.PositiveInfinity, "\"Infinity\"")]
        [InlineData(double.NaN, "\"NaN\"")]
        public void ShouldPerformSerialization(double value, string expected)
        {
            Percentage parsedValue = new(value);

            var jsonString = JsonSerializer.Serialize(parsedValue, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            jsonString.Should().Be(expected);
        }

        [Theory]
        [InlineData("0.575", 0.575)]
        [InlineData("10", 10)]
        [InlineData("10.5", 10.5)]
        [InlineData("\"-Infinity\"", double.NegativeInfinity)]
        [InlineData("\"Infinity\"", double.PositiveInfinity)]
        public void ShouldPerformDeserialization(string value, double expectedDouble)
        {
            Percentage expected = new(expectedDouble);

            Percentage actual = JsonSerializer.Deserialize<Percentage>(value, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDeserializationToNaN()
        {
            const string jsonString = "\"NaN\"";

            Percentage actual = JsonSerializer.Deserialize<Percentage>(jsonString, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            double.IsNaN(actual).Should().BeTrue();
        }
    }
}
