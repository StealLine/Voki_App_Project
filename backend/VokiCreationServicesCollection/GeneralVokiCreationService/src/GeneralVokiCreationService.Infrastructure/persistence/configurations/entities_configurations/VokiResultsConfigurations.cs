using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.results;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;

public class VokiResultsConfigurations : IEntityTypeConfiguration<VokiResult>
{
    public void Configure(EntityTypeBuilder<VokiResult> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Name)
            .HasConversion<VokiResultNameConverter>();

        builder
            .Property(x => x.Text)
            .HasConversion<VokiResultTextConverter>();

        builder
            .Property(x => x.Image)
            .HasConversion<VokiResultImageConverter>();

        builder.Property(x => x.CreationDate);
    }
}