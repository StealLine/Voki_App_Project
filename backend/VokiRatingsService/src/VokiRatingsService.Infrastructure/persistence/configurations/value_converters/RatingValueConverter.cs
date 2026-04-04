using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokiRatingsService.Domain.common;

namespace VokiRatingsService.Infrastructure.persistence.configurations.value_converters;

internal class RatingValueConverter : ValueConverter<RatingValue, ushort>
{
    public RatingValueConverter() : base(
        r => r.Value,
        v => RatingValue.Create(v).AsSuccess()
    ) { }
}