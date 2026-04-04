using System.Collections.Immutable;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Infrastructure.persistence;
using VokiCreationServicesLib.Infrastructure.persistence.value_converters;
using VokimiStorageKeysLib.concrete_keys;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;

public class DraftGeneralVokisConfigurations : IEntityTypeConfiguration<DraftGeneralVoki>
{
    public void Configure(EntityTypeBuilder<DraftGeneralVoki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Name)
            .HasConversion<VokiNameConverter>();

        builder
            .Property(x => x.Cover)
            .HasConversion<StorageKeyConverter<VokiCoverKey>>();


        builder.ComplexProperty(x => x.Details, b => {
            b.Property(d => d.Language).HasColumnName("Details_Language");
            b.Property(d => d.HasMatureContent).HasColumnName("Details_HasMatureContent");
            b
                .Property(d => d.Description)
                .HasColumnName("Details_Description")
                .HasConversion<VokiDescriptionConverter>();
        });

        builder
            .Property(x => x.Tags)
            .HasVokiTagsSetConversion();

        builder
            .Property(x => x.PrimaryAuthorId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property<VokiCoAuthorIdsSet>("CoAuthors")
            .HasConversion<VokiCoAuthorIdsSetConverter>();

        builder
            .Property<ImmutableHashSet<AppUserId>>("UserIdsToBecomeManagers")
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property(x => x.CreationDate);

        builder.ComplexProperty(x => x.TakingProcessSettings, b => {
            b.Property(s => s.ShuffleQuestions);
            b.Property(d => d.ForceSequentialAnswering);
        });

        builder.HasInteractionSettingsAsComplexProperty(x => x.InteractionSettings);

        builder
            .HasMany<VokiQuestion>("_questions")
            .WithOne()
            .HasForeignKey("VokiId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<VokiResult>("_results")
            .WithOne()
            .HasForeignKey("VokiId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}