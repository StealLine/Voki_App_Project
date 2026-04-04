using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.app_user_aggregate;
using VokisCatalogService.Infrastructure.persistence.configurations.extensions;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations;

internal class UserTakenVokisListsConfigurations : IEntityTypeConfiguration<UserTakenVokisList>
{
    public void Configure(EntityTypeBuilder<UserTakenVokisList> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Value)
            .HasUserIdToTakenVokiDataDictionaryConversion();
    }
}