using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Units.Mass;

namespace Units.EntityFramework.ValueConverters.Mass;

public class CubicMetreNullValueConverter : ValueConverter<CubicMetre?, double?>
{
    public CubicMetreNullValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<CubicMetre?, double?>> ToProvider = (CubicMetre? v) => v == null ? null : v.Value;

    private static readonly Expression<Func<double?, CubicMetre?>> FromProvider = (double? v) => v == null ? null : new CubicMetre(v.Value);
}

public class CubicMetreValueConverter : ValueConverter<CubicMetre, double>
{
    public CubicMetreValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<CubicMetre, double>> ToProvider = (CubicMetre v) => v;

    private static readonly Expression<Func<double, CubicMetre>> FromProvider = (double v) => new CubicMetre(v);
}
