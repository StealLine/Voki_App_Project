using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokisConfigurations : IEntityTypeConfiguration<Voki>
{
    public void Configure(EntityTypeBuilder<Voki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.PrimaryAuthorId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.ManagersSet)
            .HasConversion<VokiManagersIdsSetConverter>();

        builder.Property(x => x.PublicationDate);
    }
}