using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;
using UserProfilesService.Infrastructure.persistence.configurations.value_converters;

namespace UserProfilesService.Infrastructure.persistence.configurations.extensions;

public static class PropertyBuilderExtensions
{
    public static ComplexTypePropertyBuilder<HashSet<Language>> HasLanguagesHashSetConversion(
        this ComplexTypePropertyBuilder<HashSet<Language>> builder
    ) {
        return builder.HasConversion(
            new LanguagesHashSetConverter(),
            new LanguagesHashSetComparer()
        );
    }

    public static ComplexTypePropertyBuilder<ImmutableHashSet<VokiTagId>> HasTagIdImmutableHashSetConversion(
        this ComplexTypePropertyBuilder<ImmutableHashSet<VokiTagId>> builder
    ) {
        return builder.HasConversion(
            new TagIdImmutableHashSetConverter(),
            new TagIdImmutableHashSetComparer()
        );
    }

    public static ComplexTypePropertyBuilder<ImmutableArray<UserLink>> HasLinksArrayConversion(
        this ComplexTypePropertyBuilder<ImmutableArray<UserLink>> builder
    ) {
        return builder.HasConversion(
            new UserLinkArrayConverter(),
            new UserLinkArrayComparer()
        );
    }
}