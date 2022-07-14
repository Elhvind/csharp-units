using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Units.Mass;

namespace Units.EntityFramework.ValueConverters.Mass;

public class DensityNullValueConverter : ValueConverter<Density?, double?>
{
    public DensityNullValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Density?, double?>> ToProvider = (Density? v) => v == null ? null : v.Value;

    private static readonly Expression<Func<double?, Density?>> FromProvider = (double? v) => v == null ? null : new Density(v.Value);
}

public class DensityValueConverter : ValueConverter<Density, double>
{
    public DensityValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Density, double>> ToProvider = (Density v) => v;

    private static readonly Expression<Func<double, Density>> FromProvider = (double v) => new Density(v);
}
