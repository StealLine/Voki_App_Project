using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.common.vokis.scoring_voki;
using SharedKernel.common.vokis.tier_list_voki;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.type_specific_data;
using VokisCatalogService.Infrastructure.persistence.configurations.extensions;
using VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations.vokis;

public class BaseVokisConfigurations : IEntityTypeConfiguration<Voki>
{
    public void Configure(EntityTypeBuilder<Voki> builder)
    {
        builder.ToTable("BaseVokis");

        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder.Property(x => x.Type).HasConversion<string>();

        builder
            .Property(x => x.Name)
            .HasConversion<VokiNameConverter>();

        builder
            .Property(x => x.PublicationDate);
        builder
            .Property(x => x.Cover)
            .HasConversion<VokiCoverKeyConverter>();

        builder
            .Property(x => x.PrimaryAuthorId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.CoAuthorIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property<VokiManagersIdsSet>("ManagersSet")
            .HasConversion<VokiManagersIdsSetConverter>();

        builder.OwnsOne(v => v.Details, d =>
        {
            d.Property(p => p.Language).HasColumnName("Details_Language");
            d.Property(p => p.HasMatureContent).HasColumnName("Details_HasMatureContent");
            d.Property(p => p.Description).HasColumnName("Details_Description");
        });

        builder
            .Property(x => x.Tags)
            .HasTagIdImmutableHashSetHashSetConversion();

        builder.Property(x => x.PublicationDate);

        builder.Property(x => x.RatingsCount);
        builder.Property(x => x.CommentsCount);
        builder.Property(x => x.VokiTakingsCount);

        builder.OwnsOne(x => x.CatalogPageSettings, s =>
        {
            s.Property(p => p.ShowVokisRecommendedByPrimaryAuthor);
            s.Property(p => p.VokiIdsRecommendedByPrimaryAuthor)
                .HasGuidBasedIdsImmutableHashSetConversion();

            s.Property(p => p.ShowSimilarVokis);
            s.Property(p => p.ShowVokisByTheSameAuthors);
        });

        builder.Property(x => x.InteractionSettings)
            .HasConversion<BaseVokiInteractionSettingsConverter>()
            .HasColumnName("InteractionSettings");


        builder.Property(x => x.TypeSpecificData)
            .HasConversion<BaseVokiTypeSpecificDataConverter>()
            .HasColumnName("TypeSpecificData");

    }
}