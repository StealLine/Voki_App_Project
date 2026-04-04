using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TagsService.Infrastructure.persistence.configurations.value_converters;

public class VokiTagIdConverter : ValueConverter<VokiTagId, string>
{
    public VokiTagIdConverter() : base(
        id => id.ToString(),
        str => new VokiTagId(str)
    ) { }
}