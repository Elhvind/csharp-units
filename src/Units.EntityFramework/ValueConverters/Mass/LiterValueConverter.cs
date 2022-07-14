using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Units.Mass;

namespace Units.EntityFramework.ValueConverters.Mass;

public class LiterNullValueConverter : ValueConverter<Liter?, double?>
{
    public LiterNullValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Liter?, double?>> ToProvider = (Liter? v) => v.Value;

    private static readonly Expression<Func<double?, Liter?>> FromProvider = (double? v) => new Liter(v.Value);
}

public class LiterValueConverter : ValueConverter<Liter, double>
{
    public LiterValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Liter, double>> ToProvider = (Liter v) => v;

    private static readonly Expression<Func<double, Liter>> FromProvider = (double v) => new Liter(v);
}
