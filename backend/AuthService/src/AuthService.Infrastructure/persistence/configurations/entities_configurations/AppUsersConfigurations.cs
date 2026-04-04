using AuthService.Domain.app_user_aggregate;
using AuthService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.persistence.configurations.entities_configurations;

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
            .Property(x => x.Email)
            .HasConversion<EmailConverter>()
            .HasColumnType("citext");

        builder
            .Property(x => x.UniqueName)
            .HasConversion<UserUniqueNameConverter>();

        builder.Property(x => x.PasswordHash);
        builder.Property(x => x.RegistrationDate);
        builder.Property(x => x.PasswordUpdateDate);

        //indexes

        builder
            .HasIndex(x => x.UniqueName)
            .IsUnique();

        builder
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}