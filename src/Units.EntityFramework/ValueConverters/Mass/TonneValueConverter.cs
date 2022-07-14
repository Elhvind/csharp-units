using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Units.Mass;

namespace Units.EntityFramework.ValueConverters.Mass;

public class TonneNullValueConverter : ValueConverter<Tonne?, double?>
{
    public TonneNullValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Tonne?, double?>> ToProvider = (Tonne? v) => v == null ? null : v.Value;

    private static readonly Expression<Func<double?, Tonne?>> FromProvider = (double? v) => v == null ? null : new Tonne(v.Value);
}

public class TonneValueConverter : ValueConverter<Tonne, double>
{
    public TonneValueConverter(ConverterMappingHints? mappingHints = null) : base(ToProvider, FromProvider, mappingHints)
    {
    }

    private static readonly Expression<Func<Tonne, double>> ToProvider = (Tonne v) => v;

    private static readonly Expression<Func<double, Tonne>> FromProvider = (double v) => new Tonne(v);
}
