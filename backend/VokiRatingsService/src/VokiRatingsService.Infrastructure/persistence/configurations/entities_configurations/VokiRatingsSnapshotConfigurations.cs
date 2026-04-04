using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokiRatingsSnapshotConfigurations : IEntityTypeConfiguration<VokiRatingsSnapshot>
{
    public void Configure(EntityTypeBuilder<VokiRatingsSnapshot> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.VokiId)
            .HasGuidBasedIdConversion();
        
        builder.Property(x => x.Date);
        
        builder.ComplexProperty(
            x => x.Distribution,
            c => {
                c.Property(d => d.Rating1Count);
                c.Property(d => d.Rating2Count);
                c.Property(d => d.Rating3Count);
                c.Property(d => d.Rating4Count);
                c.Property(d => d.Rating5Count);
                c.Ignore(d => d.TotalCount);
                c.Ignore(d => d.TotalSum);

            }
        );
    }
}