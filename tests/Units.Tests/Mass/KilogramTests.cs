using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Units.Mass;

namespace Units.Tests.Mass;

public class KilogramTests
{
    public class StructTests
    {
        [Fact]
        public void ShouldReturnDefaultValueWhenNull()
        {
            Kilogram? value = null;
            Kilogram expected = new(0);

            Kilogram actual = value.GetValueOrDefault();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldConvertToLiterGivenDensity()
        {
            Kilogram kilogram = new(20);
            Density density = new(100);
            Liter expected = new(200);

            var actual = kilogram.InLiter(density);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnValueInLiter()
        {
            Kilogram kilogram = new(20);
            Density density = new(100);
            Liter expectedResult = new(200);

            Liter actualResult = kilogram.InLiter(density);

            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public void ShouldPerformAddition()
        {
            var left = new Kilogram(10.5);
            var right = new Kilogram(10.5);
            var expected = new Kilogram(21);

            var actual = left + right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformSubtraction()
        {
            var left = new Kilogram(10.5);
            var right = new Kilogram(5.5);
            var expected = new Kilogram(5);

            var actual = left - right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformMultiplication()
        {
            var left = new Kilogram(10.5);
            var right = new Kilogram(10);
            var expected = new Kilogram(105);

            var actual = left * right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDivision()
        {
            var left = new Kilogram(10);
            var right = new Kilogram(5);
            var expected = new Kilogram(2);

            var actual = left / right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformImplicitConversionToDouble()
        {
            const double expected = 10;
            double actual = new Kilogram(10);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformExplicitConversion()
        {
            const double valueDouble = 10d;
            const float valueFloat = 10f;
            const int valueInt = 10;
            double? valueDoubleNull = null;
            float? valueFloatNull = null;
            int? valueIntNull = null;

            var expected = new Kilogram(valueDouble);

            var actualDouble = (Kilogram)valueDouble;
            var actualFloat = (Kilogram)valueFloat;
            var actualInteger = (Kilogram)valueInt;
            var actualDoubleNull = (Kilogram?)valueDoubleNull;
            var actualFloatNull = (Kilogram?)valueFloatNull;
            var actualIntegerNull = (Kilogram?)valueIntNull;

            actualDouble.Should().Be(expected);
            actualFloat.Should().Be(expected);
            actualInteger.Should().Be(expected);
            actualDoubleNull.Should().BeNull();
            actualFloatNull.Should().BeNull();
            actualIntegerNull.Should().BeNull();
        }

        [Theory]
        [InlineData(10.5, 10.5, true)]
        [InlineData(5.5, 10.5, false)]
        [InlineData(10.5, 5.5, false)]
        public void ShouldBeEqualUsingEquals(double leftDouble, double rightDouble, bool equal)
        {
            Kilogram left = new(leftDouble);
            Kilogram right = new(rightDouble);

            var actual = left.Equals(right);

            actual.Should().Be(equal);
        }

        [Theory]
        [InlineData(10.5, 10.5, true, false, false, true, false, true)]
        [InlineData(5.5, 10.5, false, true, true, true, false, false)]
        [InlineData(10.5, 5.5, false, true, false, false, true, true)]
        public void ShouldPerformLogicalOperations(double leftDouble, double rightDouble, bool equal, bool notEqual, bool greater, bool greaterOrEqual, bool less, bool lessOrEqual)
        {
            Kilogram left = new(leftDouble);
            Kilogram right = new(rightDouble);

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
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Kilogram));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Kilogram>();
            if (obj is Kilogram actual)
                actual.Should().Be(new Kilogram(value));
        }

        [Theory]
        [InlineData(0.575)]
        [InlineData(10)]
        [InlineData(10.5)]
        public void ConvertFromDouble(double value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Kilogram));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Kilogram>();
            if (obj is Kilogram actual)
                actual.Should().Be(new Kilogram(value));
        }

        [Theory]
        [InlineData("0.575", 0.575)]
        [InlineData("10", 10)]
        [InlineData("10.5", 10.5)]
        public void ConvertFromString(string value, double expected)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Kilogram));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Kilogram>();
            if (obj is Kilogram actual)
                actual.Should().Be(new Kilogram(expected));
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
            Kilogram parsedValue = new(value);

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
            Kilogram expected = new(expectedDouble);

            Kilogram actual = JsonSerializer.Deserialize<Kilogram>(value, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDeserializationToNaN()
        {
            const string jsonString = "\"NaN\"";

            Kilogram actual = JsonSerializer.Deserialize<Kilogram>(jsonString, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            double.IsNaN(actual).Should().BeTrue();
        }
    }
}
