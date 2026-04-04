using CoreVokiCreationService.Domain.app_user_aggregate;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;

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
            .Property(x => x.InitializedVokiIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property(x => x.CoAuthoredVokiIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property(x => x.InvitedToCoAuthorVokiIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}