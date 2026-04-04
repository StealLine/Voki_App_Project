using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiCommentsService.Domain.app_user_aggregate;

namespace VokiCommentsService.Infrastructure.persistence.configurations.entities_configurations;

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
            .Property(x=>x.CommentIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}