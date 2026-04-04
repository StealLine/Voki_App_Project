using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain;
using VokiRatingsService.Domain.update_ratings_snapshot_marker_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class UpdateRatingsSnapshotMarkerConfigurations : IEntityTypeConfiguration<UpdateRatingsSnapshotMarker>
{
    public void Configure(EntityTypeBuilder<UpdateRatingsSnapshotMarker> builder) {
        builder.HasKey(m => m.Id);

        builder
            .Property(m => m.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(m => m.VokiId)
            .HasGuidBasedIdConversion();
    }
}