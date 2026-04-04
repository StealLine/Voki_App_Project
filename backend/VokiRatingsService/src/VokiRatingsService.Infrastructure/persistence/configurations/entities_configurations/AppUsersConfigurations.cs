using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class AppUsersConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.RatingIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property<ImmutableHashSet<VokiId>>("TakenVokiIds")
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}