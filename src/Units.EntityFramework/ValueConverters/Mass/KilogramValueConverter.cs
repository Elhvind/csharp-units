using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Units.Mass;

namespace Units.EntityFramework.ValueConverters.Mass;

public class KilogramNullValueConverter : ValueConverter<Kilogram?, double?>
{
    public KilogramNullValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Kilogram?, double?>> ToProvider = (Kilogram? v) => v == null ? null : v.Value;

    private static readonly Expression<Func<double?, Kilogram?>> FromProvider = (double? v) => v == null ? null : new Kilogram(v.Value);
}

public class KilogramValueConverter : ValueConverter<Kilogram, double>
{
    public KilogramValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Kilogram, double>> ToProvider = (Kilogram v) => v;

    private static readonly Expression<Func<double, Kilogram>> FromProvider = (double v) => new Kilogram(v);
}
