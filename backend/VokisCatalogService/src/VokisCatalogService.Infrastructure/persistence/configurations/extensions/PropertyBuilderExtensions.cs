using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.app_user_aggregate;
using VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

namespace VokisCatalogService.Infrastructure.persistence.configurations.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableHashSet<VokiTagId>> HasTagIdImmutableHashSetHashSetConversion(
        this PropertyBuilder<ImmutableHashSet<VokiTagId>> builder
    )
    {
        return builder.HasConversion(
            new TagIdImmutableHashSetHashSetConverter(),
            new TagIdImmutableHashSetHashSetComparer()
        );
    }
    public static PropertyBuilder<ImmutableDictionary<VokiId, UserTakenVokiData>> HasUserIdToTakenVokiDataDictionaryConversion(
        this PropertyBuilder<ImmutableDictionary<VokiId, UserTakenVokiData>> builder
    )
    {
        return builder.HasConversion(
            new UserIdToTakenVokiDataDictionaryConverter(),
            new UserIdToTakenVokiDataDictionaryComparer()
        );
    }

}