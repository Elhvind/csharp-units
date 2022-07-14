using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Units.Mass;

/// <summary>
/// The liter is a non-SI unit of volume.
/// </summary>
/// <remarks>
/// <para>SI base unit: 10^-3 m3</para>
/// <para>Symbol: L or l</para>
/// </remarks>
[TypeConverter(typeof(LiterTypeConverter))]
[JsonConverter(typeof(LiterJsonConverter))]
public readonly struct Liter :
    IComparable,
    IComparable<Liter>,
    IConvertible,
    IEquatable<Liter>
{
    private readonly double _value;

    public Liter(double value)
    {
        _value = value;
    }

    public Kilogram InKilogram(Density density) => new(_value * density / 1000);

    public CubicMetre InCubicMetre() => new(_value / 1000);

    public static readonly Liter Empty = default;

    public static Liter Parse(string value) => new(double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture));

    public static bool TryParse(string value, out Liter result)
    {
        var parsed = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var output);
        result = new(output);
        return parsed;
    }

    #region Struct base

    #region Math operators

    public static Liter operator +(Liter left, Liter right) => new(left._value + right._value);

    public static Liter operator -(Liter left, Liter right) => new(left._value - right._value);

    public static Liter operator *(Liter left, Liter right) => new(left._value * right._value);

    public static Liter operator /(Liter left, Liter right) => new(left._value / right._value);

    #endregion

    #region Equality operators

    public static bool operator ==(Liter left, Liter right) => left.Equals(right);

    public static bool operator !=(Liter left, Liter right) => !(left == right);

    public static bool operator <(Liter left, Liter right) => left.CompareTo(right) < 0;

    public static bool operator <=(Liter left, Liter right) => left.CompareTo(right) <= 0;

    public static bool operator >(Liter left, Liter right) => left.CompareTo(right) > 0;

    public static bool operator >=(Liter left, Liter right) => left.CompareTo(right) >= 0;

    #endregion

    #region Conversion operators

    public static implicit operator double(Liter liter) => liter._value;

    public static explicit operator Liter(double value) => new(value);

    public static explicit operator Liter(float value) => new(value);

    public static explicit operator Liter(int value) => new(value);

    #endregion

    #region IEquatable

    public override bool Equals(object? obj)
    {
        return obj != null
            && obj.GetType() == GetType()
            && Equals((Liter)obj);
    }

    public bool Equals(Liter other) => _value == other;

    #endregion

    #region IComparable

    public int CompareTo(object? obj) => obj != null && obj.GetType() == GetType() ? CompareTo((Liter)obj) : 0;

    public int CompareTo(Liter other) => _value.CompareTo(other);

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

    public override string ToString() => $"{_value} l";

    #endregion Struct base
}

public class LiterJsonConverter : JsonConverter<Liter>
{
    public override bool CanConvert(Type objectType) => objectType == typeof(Liter) || base.CanConvert(objectType);

    public override Liter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<double>(ref reader, options);
        return new(value);
    }

    public override void Write(Utf8JsonWriter writer, Liter value, JsonSerializerOptions options)
    {
        var jsonString = JsonSerializer.Serialize<double>(value, options);
        writer.WriteRawValue(jsonString);
    }
}

public class LiterTypeConverter : TypeConverter
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
            ? new Liter(result)
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

        if (value is Liter liter)
        {
            if (converter.CanConvertTo(context, value.GetType()))
                return converter.ConvertTo(context, culture, (double)liter, destinationType);

            if (destinationType == typeof(string))
                return liter.ToString();

            if (destinationType == typeof(double))
                return liter.ToDouble(null);

            if (destinationType == typeof(int))
                return liter.ToInt32(null);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
