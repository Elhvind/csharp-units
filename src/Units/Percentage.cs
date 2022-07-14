using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Units.Mass;

namespace Units;

/// <summary>
/// Percentage is a number or ratio expressed as a fraction of 100.
/// </summary>
[TypeConverter(typeof(PercentageTypeConverter))]
[JsonConverter(typeof(PercentageJsonConverter))]
public readonly struct Percentage :
    IComparable,
    IComparable<Percentage>,
    IConvertible,
    IEquatable<Percentage>
{
    private readonly double _value;

    public Percentage(double percentage)
    {
        _value = percentage;
    }

    public Percentage(double amount, double totalAmount)
        : this(totalAmount == 0 ? 0 : (amount / totalAmount) * 100)
    { }

    public double Fraction => _value / 100;

    public double Of(double value) => value * Fraction;
    public Kilogram Of(Kilogram value) => (Kilogram)Of((double)value);
    public Liter Of(Liter value) => (Liter)Of((double)value);

    public double Remainder(double value) => Total(value) - value;
    public Kilogram Remainder(Kilogram value) => Total(value) - value;
    public Liter Remainder(Liter value) => Total(value) - value;

    public double Total(double value) => value / Fraction;
    public Kilogram Total(Kilogram value) => (Kilogram)Total((double)value);
    public Liter Total(Liter value) => (Liter)Total((double)value);

    public static readonly Percentage Empty = default;

    public static Percentage Parse(string value) => new(double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture));

    public static bool TryParse(string value, out Percentage result)
    {
        var parsed = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var output);
        result = new(output);
        return parsed;
    }

    #region Struct base

    #region Math operators

    public static Percentage operator +(Percentage left, Percentage right) => new(left._value + right._value);

    public static Percentage operator -(Percentage left, Percentage right) => new(left._value - right._value);

    public static Percentage operator *(Percentage left, Percentage right) => new(left._value * right._value);

    public static Percentage operator /(Percentage left, Percentage right) => new(left._value / right._value);

    #endregion

    #region Conversion operators

    public static implicit operator double(Percentage percentage) => percentage._value;

    public static explicit operator Percentage(double value) => new(value);

    public static explicit operator Percentage(float value) => new(value);

    public static explicit operator Percentage(int value) => new(value);

    #endregion

    #region Equality operators

    public static bool operator ==(Percentage left, Percentage right) => left.Equals(right);

    public static bool operator !=(Percentage left, Percentage right) => !(left == right);

    public static bool operator <(Percentage left, Percentage right) => left.CompareTo(right) < 0;

    public static bool operator <=(Percentage left, Percentage right) => left.CompareTo(right) <= 0;

    public static bool operator >(Percentage left, Percentage right) => left.CompareTo(right) > 0;

    public static bool operator >=(Percentage left, Percentage right) => left.CompareTo(right) >= 0;

    #endregion

    #region IEquatable

    public override bool Equals(object? obj)
    {
        return obj != null
            && obj.GetType() == GetType()
            && Equals((Percentage)obj);
    }

    public bool Equals(Percentage other) => _value == other;

    #endregion

    #region IComparable

    public int CompareTo(object? obj) => obj != null && obj.GetType() == GetType() ? CompareTo((Percentage)obj) : 0;

    public int CompareTo(Percentage other) => _value.CompareTo(other);

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

    public override string ToString() => $"{_value} %";

    #endregion Struct base
}

public class PercentageJsonConverter : JsonConverter<Percentage>
{
    public override bool CanConvert(Type objectType) => objectType == typeof(Percentage) || base.CanConvert(objectType);

    public override Percentage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<double>(ref reader, options);
        return new(value);
    }

    public override void Write(Utf8JsonWriter writer, Percentage value, JsonSerializerOptions options)
    {
        var jsonString = JsonSerializer.Serialize<double>(value, options);
        writer.WriteRawValue(jsonString);
    }
}

public class PercentageTypeConverter : TypeConverter
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
            ? new Percentage(result)
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

        if (value is Percentage percentage)
        {
            if (converter.CanConvertTo(context, value.GetType()))
                return converter.ConvertTo(context, culture, (double)percentage, destinationType);

            if (destinationType == typeof(string))
                return percentage.ToString();

            if (destinationType == typeof(double))
                return percentage.ToDouble(null);

            if (destinationType == typeof(int))
                return percentage.ToInt32(null);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
