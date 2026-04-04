using System.Collections.Immutable;
using System.Linq.Expressions;
using InfrastructureShared.EfCore.value_converters.guid_based_ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.common.vokis.scoring_voki;
using SharedKernel.common.vokis.tier_list_voki;

namespace InfrastructureShared.EfCore;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasGuidBasedIdConversion<TId>(
        this PropertyBuilder<TId> builder
    ) where TId : notnull, GuidBasedId {
        return builder.HasConversion(new GuidBasedIdConverter<TId>());
    }

    public static PropertyBuilder<TId?> HasNullableGuidBasedIdConversion<TId>(
        this PropertyBuilder<TId?> builder
    ) where TId : GuidBasedId {
        return builder.HasConversion(new NullableGuidBasedIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasGuidBasedIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder
    ) where T : GuidBasedId {
        return builder.HasConversion(
            new GuidBasedIdHashSetConverter<T>(),
            new GuidBasedIdHashSetComparer<T>()
        );
    }

    public static ComplexTypePropertyBuilder<ImmutableHashSet<T>> HasGuidBasedIdsImmutableHashSetConversion<T>(
        this ComplexTypePropertyBuilder<ImmutableHashSet<T>> builder
    ) where T : GuidBasedId {
        return builder.HasConversion(
            new GuidBasedIdImmutableHashSetConverter<T>(),
            new GuidBasedIdImmutableHashSetComparer<T>()
        );
    }

    public static PropertyBuilder<ImmutableHashSet<T>> HasGuidBasedIdsImmutableHashSetConversion<T>(
        this PropertyBuilder<ImmutableHashSet<T>> builder
    ) where T : GuidBasedId {
        return builder.HasConversion(
            new GuidBasedIdImmutableHashSetConverter<T>(),
            new GuidBasedIdImmutableHashSetComparer<T>()
        );
    }

    public static EntityTypeBuilder<TEntity> HasInteractionSettingsAsComplexProperty<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, GeneralVokiInteractionSettings?>> propertyExpression
    )
        where TEntity : class {
        builder.ComplexProperty(propertyExpression, b => {
            b.Property(s => s.SignedInOnlyTaking);
            b.Property(s => s.ResultsVisibility).HasConversion<string>();
            b.Property(s => s.ShowResultsDistribution);
        });

        return builder;
    }


    public static EntityTypeBuilder<TEntity> HasInteractionSettingsAsComplexProperty<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TierListVokiInteractionSettings?>> propertyExpression
    ) where TEntity : class {
        return builder.ComplexProperty(propertyExpression,
            b => { b.Property(s => s.SignedInOnlyTaking); }
        );
    }

    public static EntityTypeBuilder<TEntity> HasInteractionSettingsAsComplexProperty<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, ScoringVokiInteractionSettings?>> propertyExpression
    ) where TEntity : class {
        return builder.ComplexProperty(propertyExpression,
            b => { b.Property(s => s.SignedInOnlyTaking); }
        );
    }
}