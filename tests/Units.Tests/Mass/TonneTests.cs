using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Units.Mass;

namespace Units.Tests.Mass;

public class TonneTests
{
    public class StructTests
    {
        [Fact]
        public void ShouldReturnDefaultValueWhenNull()
        {
            Tonne? value = null;
            Tonne expected = new(0);

            Tonne actual = value.GetValueOrDefault();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformAddition()
        {
            var left = new Tonne(10.5);
            var right = new Tonne(10.5);
            var expected = new Tonne(21);

            var actual = left + right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformSubtraction()
        {
            var left = new Tonne(10.5);
            var right = new Tonne(5.5);
            var expected = new Tonne(5);

            var actual = left - right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformMultiplication()
        {
            var left = new Tonne(10.5);
            var right = new Tonne(10);
            var expected = new Tonne(105);

            var actual = left * right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDivision()
        {
            var left = new Tonne(10);
            var right = new Tonne(5);
            var expected = new Tonne(2);

            var actual = left / right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformImplicitConversionToDouble()
        {
            const double expected = 10;
            double actual = new Tonne(10);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformExplicitConversion()
        {
            const double valueDouble = 10d;
            const float valueFloat = 10f;
            const int valueInt = 10;

            var expected = new Tonne(valueDouble);
            var actualD = (Tonne)valueDouble;
            var actualF = (Tonne)valueFloat;
            var actualI = (Tonne)valueInt;

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
            Tonne left = new(leftDouble);
            Tonne right = new(rightDouble);

            var actual = left.Equals(right);

            actual.Should().Be(equal);
        }

        [Theory]
        [InlineData(10.5, 10.5, true, false, false, true, false, true)]
        [InlineData(5.5, 10.5, false, true, true, true, false, false)]
        [InlineData(10.5, 5.5, false, true, false, false, true, true)]
        public void ShouldPerformLogicalOperations(double leftDouble, double rightDouble, bool equal, bool notEqual, bool greater, bool greaterOrEqual, bool less, bool lessOrEqual)
        {
            Tonne left = new(leftDouble);
            Tonne right = new(rightDouble);

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
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Tonne));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Tonne>();
            if (obj is Tonne actual)
                actual.Should().Be(new Tonne(value));
        }

        [Theory]
        [InlineData(0.575)]
        [InlineData(10)]
        [InlineData(10.5)]
        public void ConvertFromDouble(double value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Tonne));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Tonne>();
            if (obj is Tonne actual)
                actual.Should().Be(new Tonne(value));
        }

        [Theory]
        [InlineData("0.575", 0.575)]
        [InlineData("10", 10)]
        [InlineData("10.5", 10.5)]
        public void ConvertFromString(string value, double expected)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Tonne));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<Tonne>();
            if (obj is Tonne actual)
                actual.Should().Be(new Tonne(expected));
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
            Tonne parsedValue = new(value);

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
            Tonne expected = new(expectedDouble);

            Tonne actual = JsonSerializer.Deserialize<Tonne>(value, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDeserializationToNaN()
        {
            const string jsonString = "\"NaN\"";

            Tonne actual = JsonSerializer.Deserialize<Tonne>(jsonString, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            double.IsNaN(actual).Should().BeTrue();
        }
    }
}
