using AuthService.Domain.unconfirmed_user_aggregate;
using AuthService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.persistence.configurations.entities_configurations;

internal class UnconfirmedUsersConfigurations : IEntityTypeConfiguration<UnconfirmedUser>
{
    public void Configure(EntityTypeBuilder<UnconfirmedUser> builder) {
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
            .Property(x => x.UserUniqueName)
            .HasConversion<UserUniqueNameConverter>();

        builder.Property<string>("ConfirmationCode");

        builder.Property(x => x.ExpiresAt);

        //indexes
        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .HasIndex(x => x.UserUniqueName)
            .IsUnique();

        builder
            .HasIndex(x => x.ExpiresAt);
    }
}