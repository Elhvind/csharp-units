using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Units.Mass;

namespace Units.Tests.Mass;

public class LiterTests
{
    public class StructTests
    {
        [Fact]
        public void ShouldReturnDefaultValueWhenNull()
        {
            Liter? value = null;
            Liter expected = new(0);

            Liter actual = value.GetValueOrDefault();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldConvertToKilogramGivenDensity()
        {
            Liter liter = new(200);
            Density density = new(100);
            Kilogram expected = new(20);

            var actual = liter.InKilogram(density);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformAddition()
        {
            var left = new Liter(10.5);
            var right = new Liter(10.5);
            var expected = new Liter(21);

            var actual = left + right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnValueInKilogram()
        {
            Liter liter = new(200);
            Density density = new(100);
            Kilogram expectedResult = new(20);

            Kilogram actualResult = liter.InKilogram(density);

            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public void ShouldReturnValueInCubicMetre()
        {
            Liter liter = new(1);
            CubicMetre expectedResult = new(0.001);

            CubicMetre actualResult = liter.InCubicMetre();

            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public void ShouldPerformSubtraction()
        {
            var left = new Liter(10.5);
            var right = new Liter(5.5);
            var expected = new Liter(5);

            var actual = left - right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformMultiplication()
        {
            var left = new Liter(10.5);
            var right = new Liter(10);
            var expected = new Liter(105);

            var actual = left * right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDivision()
        {
            var left = new Liter(10);
            var right = new Liter(5);
            var expected = new Liter(2);

            var actual = left / right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformImplicitConversionToDouble()
        {
            const double expected = 10;
            double actual = new Liter(10);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformExplicitConversion()
        {
            const double valueDouble = 10d;
            const float valueFloat = 10f;
            const int valueInt = 10;

            var expected = new Liter(valueDouble);
            var actualD = (Liter)valueDouble;
            var actualF = (Liter)valueFloat;
            var actualI = (Liter)valueInt;

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
            Liter left = new(leftDouble);
            Liter right = new(rightDouble);

            var actual = left.Equals(right);

            actual.Should().Be(equal);
        }

        [Theory]
        [InlineData(10.5, 10.5, true, false, false, true, false, true)]
        [InlineData(5.5, 10.5, false, true, true, true, false, false)]
        [InlineData(10.5, 5.5, false, true, false, false, true, true)]
        public void ShouldPerformLogicalOperations(double leftDouble, double rightDouble, bool equal, bool notEqual, bool greater, bool greaterOrEqual, bool less, bool lessOrEqual)
        {
            Liter left = new(leftDouble);
            Liter right = new(rightDouble);

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
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Liter));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Liter>();
            if (obj is Liter actual)
                actual.Should().Be(new Liter(value));
        }

        [Theory]
        [InlineData(0.575)]
        [InlineData(10)]
        [InlineData(10.5)]
        public void ConvertFromDouble(double value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Liter));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Liter>();
            if (obj is Liter actual)
                actual.Should().Be(new Liter(value));
        }

        [Theory]
        [InlineData("0.575", 0.575)]
        [InlineData("10", 10)]
        [InlineData("10.5", 10.5)]
        public void ConvertFromString(string value, double expected)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Liter));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Liter>();
            if (obj is Liter actual)
                actual.Should().Be(new Liter(expected));
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
            Liter parsedValue = new(value);

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
            Liter expected = new(expectedDouble);

            Liter actual = JsonSerializer.Deserialize<Liter>(value, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDeserializationToNaN()
        {
            const string jsonString = "\"NaN\"";

            Liter actual = JsonSerializer.Deserialize<Liter>(jsonString, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            double.IsNaN(actual).Should().BeTrue();
        }
    }
}
