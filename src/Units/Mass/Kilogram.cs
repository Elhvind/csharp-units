using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Units.Mass;

/// <summary>
/// The kilogram is the SI unit of mass.
/// </summary>
/// <remarks>
/// <para>SI unit: kg</para>
/// <para>Symbol: kg</para>
/// </remarks>
[TypeConverter(typeof(KilogramTypeConverter))]
[JsonConverter(typeof(KilogramJsonConverter))]
public readonly struct Kilogram :
    IComparable,
    IComparable<Kilogram>,
    IConvertible,
    IEquatable<Kilogram>
{
    private readonly double _value;

    public Kilogram(double value)
    {
        _value = value;
    }

    public Tonne InTonne() => new(_value / 1000);

    public Liter InLiter(Density density) => new(density == 0 ? 0 : _value / density * 1000);

    public static readonly Kilogram Empty = default;

    public static Kilogram Parse(string value) => new(double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture));

    public static bool TryParse(string value, out Kilogram result)
    {
        var parsed = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var output);
        result = new(output);
        return parsed;
    }

    #region Struct base

    #region Math operators

    public static Kilogram operator +(Kilogram left, Kilogram right) => new(left._value + right._value);

    public static Kilogram operator -(Kilogram left, Kilogram right) => new(left._value - right._value);

    public static Kilogram operator *(Kilogram left, Kilogram right) => new(left._value * right._value);

    public static Kilogram operator /(Kilogram left, Kilogram right) => new(left._value / right._value);

    #endregion

    #region Equality operators

    public static bool operator ==(Kilogram left, Kilogram right) => left.Equals(right);

    public static bool operator !=(Kilogram left, Kilogram right) => !(left == right);

    public static bool operator <(Kilogram left, Kilogram right) => left.CompareTo(right) < 0;

    public static bool operator <=(Kilogram left, Kilogram right) => left.CompareTo(right) <= 0;

    public static bool operator >(Kilogram left, Kilogram right) => left.CompareTo(right) > 0;

    public static bool operator >=(Kilogram left, Kilogram right) => left.CompareTo(right) >= 0;

    #endregion

    #region Conversion operators

    public static implicit operator double(Kilogram kilos) => kilos._value;

    public static explicit operator Kilogram(double value) => new(value);

    public static explicit operator Kilogram(float value) => new(value);

    public static explicit operator Kilogram(int value) => new(value);

    #endregion

    #region IEquatable

    public override bool Equals(object? obj)
    {
        return obj != null
            && obj.GetType() == GetType()
            && Equals((Kilogram)obj);
    }

    public bool Equals(Kilogram other) => _value == other;

    #endregion

    #region IComparable

    public int CompareTo(object? obj) => obj != null && obj.GetType() == GetType() ? CompareTo((Kilogram)obj) : 0;

    public int CompareTo(Kilogram other) => _value.CompareTo(other);

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

    public override string ToString() => $"{_value} kg";

    #endregion Struct base
}

public class KilogramJsonConverter : JsonConverter<Kilogram>
{
    public override bool CanConvert(Type objectType) => objectType == typeof(Kilogram) || base.CanConvert(objectType);

    public override Kilogram Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<double>(ref reader, options);
        return new(value);
    }

    public override void Write(Utf8JsonWriter writer, Kilogram value, JsonSerializerOptions options)
    {
        var jsonString = JsonSerializer.Serialize<double>(value, options);
        writer.WriteRawValue(jsonString);
    }
}

public class KilogramTypeConverter : TypeConverter
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
            ? new Kilogram(result)
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

        if (value is Kilogram kilogram)
        {
            if (converter.CanConvertTo(context, value.GetType()))
                return converter.ConvertTo(context, culture, (double)kilogram, destinationType);

            if (destinationType == typeof(string))
                return kilogram.ToString();

            if (destinationType == typeof(double))
                return kilogram.ToDouble(null);

            if (destinationType == typeof(int))
                return kilogram.ToInt32(null);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
