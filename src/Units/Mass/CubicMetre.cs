using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Units.Mass;

/// <summary>
/// The cubic metre is the SI unit of volume.
/// </summary>
/// <remarks>
/// <para>SI base unit: m3</para>
/// <para>Symbol: m3</para>
/// </remarks>
[TypeConverter(typeof(CubicMetreTypeConverter))]
[JsonConverter(typeof(CubicMetreJsonConverter))]
public readonly struct CubicMetre :
    IComparable,
    IComparable<CubicMetre>,
    IConvertible,
    IEquatable<CubicMetre>
{
    private readonly double _value;

    public CubicMetre(double value)
    {
        _value = value;
    }

    public Liter InLiter() => new(_value * 1000);

    public static readonly CubicMetre Empty = default;

    public static CubicMetre Parse(string value) => new(double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture));

    public static bool TryParse(string value, out CubicMetre result)
    {
        var parsed = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var output);
        result = new(output);
        return parsed;
    }

    #region Struct base

    #region Math operators

    public static CubicMetre operator +(CubicMetre left, CubicMetre right) => new(left._value + right._value);

    public static CubicMetre operator -(CubicMetre left, CubicMetre right) => new(left._value - right._value);

    public static CubicMetre operator *(CubicMetre left, CubicMetre right) => new(left._value * right._value);

    public static CubicMetre operator /(CubicMetre left, CubicMetre right) => new(left._value / right._value);

    #endregion

    #region Equality operators

    public static bool operator ==(CubicMetre left, CubicMetre right) => left.Equals(right);

    public static bool operator !=(CubicMetre left, CubicMetre right) => !(left == right);

    public static bool operator <(CubicMetre left, CubicMetre right) => left.CompareTo(right) < 0;

    public static bool operator <=(CubicMetre left, CubicMetre right) => left.CompareTo(right) <= 0;

    public static bool operator >(CubicMetre left, CubicMetre right) => left.CompareTo(right) > 0;

    public static bool operator >=(CubicMetre left, CubicMetre right) => left.CompareTo(right) >= 0;

    #endregion

    #region Conversion operators

    public static implicit operator double(CubicMetre cubicMetre) => cubicMetre._value;

    public static explicit operator CubicMetre(double value) => new(value);

    public static explicit operator CubicMetre(float value) => new(value);

    public static explicit operator CubicMetre(int value) => new(value);

    #endregion

    #region IEquatable

    public override bool Equals(object? obj)
    {
        return obj != null
            && obj.GetType() == GetType()
            && Equals((CubicMetre)obj);
    }

    public bool Equals(CubicMetre other) => _value == other;

    #endregion

    #region IComparable

    public int CompareTo(object? obj) => obj != null && obj.GetType() == GetType() ? CompareTo((CubicMetre)obj) : 0;

    public int CompareTo(CubicMetre other) => _value.CompareTo(other);

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

    public override string ToString() => $"{_value} m3";

    #endregion Struct base
}

public class CubicMetreJsonConverter : JsonConverter<CubicMetre>
{
    public override bool CanConvert(Type objectType) => objectType == typeof(CubicMetre) || base.CanConvert(objectType);

    public override CubicMetre Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<double>(ref reader, options);
        return new(value);
    }

    public override void Write(Utf8JsonWriter writer, CubicMetre value, JsonSerializerOptions options)
    {
        var jsonString = JsonSerializer.Serialize<double>(value, options);
        writer.WriteRawValue(jsonString);
    }
}

public class CubicMetreTypeConverter : TypeConverter
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
            ? new CubicMetre(result)
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

        if (value is CubicMetre cubicMetre)
        {
            if (converter.CanConvertTo(context, value.GetType()))
                return converter.ConvertTo(context, culture, (double)cubicMetre, destinationType);

            if (destinationType == typeof(string))
                return cubicMetre.ToString();

            if (destinationType == typeof(double))
                return cubicMetre.ToDouble(null);

            if (destinationType == typeof(int))
                return cubicMetre.ToInt32(null);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
