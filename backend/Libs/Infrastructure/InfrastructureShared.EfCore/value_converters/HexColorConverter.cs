using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common;

namespace InfrastructureShared.EfCore.value_converters;
public class HexColorConverter : ValueConverter<HexColor, string>
{
    public HexColorConverter() : base(
        id => id.ToString(),
        value => HexColor.Create(value).AsSuccess()
    ) { }
}