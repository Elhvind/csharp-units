using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Units.Mass;

namespace Units.Tests.Mass;

public class CubicMetreTests
{
    public class StructTests
    {
        [Fact]
        public void ShouldConvertToLiter()
        {
            CubicMetre cubicMetre = new(1);
            Liter expectedLiter = new(1000);

            Liter actualLiter = cubicMetre.InLiter();

            actualLiter.Should().Be(expectedLiter);
        }

        [Fact]
        public void ShouldReturnDefaultValueWhenNull()
        {
            CubicMetre? value = null;
            CubicMetre expected = new(0);

            CubicMetre actual = value.GetValueOrDefault();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformAddition()
        {
            var left = new CubicMetre(10.5);
            var right = new CubicMetre(10.5);
            var expected = new CubicMetre(21);

            var actual = left + right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformSubtraction()
        {
            var left = new CubicMetre(10.5);
            var right = new CubicMetre(5.5);
            var expected = new CubicMetre(5);

            var actual = left - right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformMultiplication()
        {
            var left = new CubicMetre(10.5);
            var right = new CubicMetre(10);
            var expected = new CubicMetre(105);

            var actual = left * right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDivision()
        {
            var left = new CubicMetre(10);
            var right = new CubicMetre(5);
            var expected = new CubicMetre(2);

            var actual = left / right;

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformImplicitConversionToDouble()
        {
            const double expected = 10;
            double actual = new CubicMetre(10);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformExplicitConversion()
        {
            const double valueDouble = 10d;
            const float valueFloat = 10f;
            const int valueInt = 10;

            var expected = new CubicMetre(valueDouble);
            var actualD = (CubicMetre)valueDouble;
            var actualF = (CubicMetre)valueFloat;
            var actualI = (CubicMetre)valueInt;

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
            CubicMetre left = new(leftDouble);
            CubicMetre right = new(rightDouble);

            var actual = left.Equals(right);

            actual.Should().Be(equal);
        }

        [Theory]
        [InlineData(10.5, 10.5, true, false, false, true, false, true)]
        [InlineData(5.5, 10.5, false, true, true, true, false, false)]
        [InlineData(10.5, 5.5, false, true, false, false, true, true)]
        public void ShouldPerformLogicalOperations(double leftDouble, double rightDouble, bool equal, bool notEqual, bool greater, bool greaterOrEqual, bool less, bool lessOrEqual)
        {
            CubicMetre left = new(leftDouble);
            CubicMetre right = new(rightDouble);

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
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(CubicMetre));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<CubicMetre>();
            if (obj is CubicMetre actual)
                actual.Should().Be(new CubicMetre(value));
        }

        [Theory]
        [InlineData(0.575)]
        [InlineData(10)]
        [InlineData(10.5)]
        public void ConvertFromDouble(double value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(CubicMetre));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<CubicMetre>();
            if (obj is CubicMetre actual)
                actual.Should().Be(new CubicMetre(value));
        }

        [Theory]
        [InlineData("0.575", 0.575)]
        [InlineData("10", 10)]
        [InlineData("10.5", 10.5)]
        public void ConvertFromString(string value, double expected)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(CubicMetre));

            object? obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            obj.Should().NotBeNull().And.BeOfType<CubicMetre>();
            if (obj is CubicMetre actual)
                actual.Should().Be(new CubicMetre(expected));
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
            CubicMetre parsedValue = new(value);

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
            CubicMetre expected = new(expectedDouble);

            CubicMetre actual = JsonSerializer.Deserialize<CubicMetre>(value, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPerformDeserializationToNaN()
        {
            const string jsonString = "\"NaN\"";

            CubicMetre actual = JsonSerializer.Deserialize<CubicMetre>(jsonString, new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });

            double.IsNaN(actual).Should().BeTrue();
        }
    }
}
