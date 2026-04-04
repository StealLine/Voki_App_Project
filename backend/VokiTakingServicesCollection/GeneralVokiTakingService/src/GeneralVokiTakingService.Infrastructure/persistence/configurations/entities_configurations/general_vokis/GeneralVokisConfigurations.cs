using GeneralVokiTakingService.Domain.general_voki_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.common.vokis;
using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.general_vokis;

public class GeneralVokisConfigurations : IEntityTypeConfiguration<GeneralVoki>
{
    public void Configure(EntityTypeBuilder<GeneralVoki> builder) {
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
            .HasMany<VokiQuestion>("Questions")
            .WithOne()
            .HasForeignKey("VokiId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<bool>("ForceSequentialAnswering");
        builder.Property(x => x.ShuffleQuestions);

        builder
            .HasMany<VokiResult>("_results")
            .WithOne()
            .HasForeignKey("VokiId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property<ImmutableHashSet<VokiTakenRecordId>>("VokiTakenRecordIds")
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property<VokiManagersIdsSet>("ManagersSet")
            .HasConversion<VokiManagersIdsSetConverter>();

        builder.HasInteractionSettingsAsComplexProperty(x => x.InteractionSettings);
    }
}