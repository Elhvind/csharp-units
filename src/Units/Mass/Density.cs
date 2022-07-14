using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Units.Mass;

/// <summary>
/// The density of a substance is its mass per unit volume.
/// </summary>
/// <remarks>
/// <para>SI unit: kg/m3</para>
/// <para>Symbol: kg/m3</para>
/// </remarks>
[TypeConverter(typeof(DensityTypeConverter))]
[JsonConverter(typeof(DensityJsonConverter))]
public readonly struct Density :
    IComparable,
    IComparable<Density>,
    IConvertible,
    IEquatable<Density>
{
    private readonly double _value;

    public Density(double value)
    {
        _value = value;
    }

    public Density(Kilogram kilogram, Liter liter)
        : this(liter == 0 ? 0 : kilogram / liter * 1000)
    { }

    public static readonly Density Empty = default;

    public static readonly Density Water = new(1000);

    public static Density Parse(string value) => new(double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture));

    public static bool TryParse(string value, out Density result)
    {
        var parsed = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var output);
        result = new(output);
        return parsed;
    }

    #region Struct base

    #region Math operators

    public static Density operator +(Density left, Density right) => new(left._value + right._value);

    public static Density operator -(Density left, Density right) => new(left._value - right._value);

    public static Density operator *(Density left, Density right) => new(left._value * right._value);

    public static Density operator /(Density left, Density right) => new(left._value / right._value);

    #endregion

    #region Conversion operators

    public static implicit operator double(Density gram) => gram._value;

    public static explicit operator Density(double value) => new(value);

    public static explicit operator Density(float value) => new(value);

    public static explicit operator Density(int value) => new(value);

    #endregion

    #region Equality operators

    public static bool operator ==(Density left, Density right) => left.Equals(right);

    public static bool operator !=(Density left, Density right) => !(left == right);

    public static bool operator <(Density left, Density right) => left.CompareTo(right) < 0;

    public static bool operator <=(Density left, Density right) => left.CompareTo(right) <= 0;

    public static bool operator >(Density left, Density right) => left.CompareTo(right) > 0;

    public static bool operator >=(Density left, Density right) => left.CompareTo(right) >= 0;

    #endregion

    #region IEquatable

    public override bool Equals(object? obj)
    {
        return obj != null
            && obj.GetType() == GetType()
            && Equals((Density)obj);
    }

    public bool Equals(Density other) => _value == other;

    #endregion

    #region IComparable

    public int CompareTo(object? obj) => obj != null && obj.GetType() == GetType() ? CompareTo((Density)obj) : 0;

    public int CompareTo(Density other) => _value.CompareTo(other);

    #endregion

    #region IConvertible

    public TypeCode GetTypeCode() => _value.GetTypeCode();

    public bool ToBoolean(IFormatProvider? provider) => ((IConvertible)_value).ToBoolean(provider);

    public byte ToByte(IFormatProvider? provider) => ((IConvertible)_value).ToByte(provider);

    public char ToChar(IFormatProvider? provider) => ((IConvertible)_value).ToChar(provider);

    public DateTime ToDateTime(IFormatProvider? provider) => ((IConvertible)_value).ToDateTime(provider);

    public decimal ToDecimal(IFormatProvider? provider) => ((IConvertible)_value).ToDecimal(provider);

    public double ToDouble(IFormatProvider? provider) => ((IConvertible)_value).ToDouble(provider);

    public short ToInt16(IFormatProvider? provider) => ((IConvertible)_value).ToInt16(provider);

    public int ToInt32(IFormatProvider? provider) => ((IConvertible)_value).ToInt32(provider);

    public long ToInt64(IFormatProvider? provider) => ((IConvertible)_value).ToInt64(provider);

    public sbyte ToSByte(IFormatProvider? provider) => ((IConvertible)_value).ToSByte(provider);

    public float ToSingle(IFormatProvider? provider) => ((IConvertible)_value).ToSingle(provider);

    public string ToString(IFormatProvider? provider) => _value.ToString(provider);

    public object ToType(Type conversionType, IFormatProvider? provider) => ((IConvertible)_value).ToType(conversionType, provider);

    public ushort ToUInt16(IFormatProvider? provider) => ((IConvertible)_value).ToUInt16(provider);

    public uint ToUInt32(IFormatProvider? provider) => ((IConvertible)_value).ToUInt32(provider);

    public ulong ToUInt64(IFormatProvider? provider) => ((IConvertible)_value).ToUInt64(provider);

    #endregion

    public override int GetHashCode() => _value.GetHashCode();

    public override string ToString() => $"{_value} kg/m3";

    #endregion Struct base
}

public class DensityJsonConverter : JsonConverter<Density>
{
    public override bool CanConvert(Type objectType) => objectType == typeof(Density) || base.CanConvert(objectType);

    public override Density Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<double>(ref reader, options);
        return new(value);
    }

    public override void Write(Utf8JsonWriter writer, Density value, JsonSerializerOptions options)
    {
        var jsonString = JsonSerializer.Serialize<double>(value, options);
        writer.WriteRawValue(jsonString);
    }
}

public class DensityTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
            || sourceType == typeof(double)
            || sourceType == typeof(int)
            || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        object? convertedValue = null;
        var converter = TypeDescriptor.GetConverter(typeof(double));
        if (converter.CanConvertFrom(context, value.GetType()))
        {
            convertedValue = converter.ConvertFrom(context, culture, value);
        }
        else if (value is double doubleValue)
        {
            convertedValue = doubleValue;
        }
        else if (value is int intValue)
        {
            convertedValue = (double)intValue;
        }
        return convertedValue is double result
            ? new Density(result)
            : base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string)
            || destinationType == typeof(double)
            || destinationType == typeof(int)
            || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        var converter = TypeDescriptor.GetConverter(typeof(double));

        if (value is Density density)
        {
            if (converter.CanConvertTo(context, value.GetType()))
                return converter.ConvertTo(context, culture, (double)density, destinationType);

            if (destinationType == typeof(string))
                return density.ToString();

            if (destinationType == typeof(double))
                return density.ToDouble(null);

            if (destinationType == typeof(int))
                return density.ToInt32(null);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
