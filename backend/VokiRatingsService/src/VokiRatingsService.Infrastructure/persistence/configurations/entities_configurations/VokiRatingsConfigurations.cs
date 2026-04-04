using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.voki_rating_aggregate;
using VokiRatingsService.Infrastructure.persistence.configurations.value_converters;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokiRatingsConfigurations : IEntityTypeConfiguration<VokiRating>
{
    public void Configure(EntityTypeBuilder<VokiRating> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.UserId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.VokiId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.CurrentValue)
            .HasConversion<RatingValueConverter>();
        builder.Property(x => x.LastUpdated);

        builder
            .Property(x => x.InitialValue)
            .HasConversion<RatingValueConverter>();
        builder.Property(x => x.InitialDateTime);

        builder.Ignore(x => x.WasUpdated);
    }
}