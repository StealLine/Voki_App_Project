using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Infrastructure.persistence.value_converters;

namespace VokiCreationServicesLib.Infrastructure.persistence;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<VokiTagsSet> HasVokiTagsSetConversion(this PropertyBuilder<VokiTagsSet> builder) {
        return builder.HasConversion(
            new VokiTagsSetConverter(),
            new VokiTagsSetComparer()
        );
    }
}