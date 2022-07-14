using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Units.EntityFramework.ValueConverters;

public class PercentageNullValueConverter : ValueConverter<Percentage?, double?>
{
    public PercentageNullValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Percentage?, double?>> ToProvider = (Percentage? v) => v.Value;

    private static readonly Expression<Func<double?, Percentage?>> FromProvider = (double? v) => new Percentage(v.Value);
}

public class PercentageValueConverter : ValueConverter<Percentage, double>
{
    public PercentageValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Percentage, double>> ToProvider = (Percentage v) => v;

    private static readonly Expression<Func<double, Percentage>> FromProvider = (double v) => new Percentage(v);
}
