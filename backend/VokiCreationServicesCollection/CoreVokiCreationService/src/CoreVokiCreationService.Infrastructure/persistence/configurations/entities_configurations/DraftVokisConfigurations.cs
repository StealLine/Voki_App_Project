using CoreVokiCreationService.Domain.draft_voki_aggregate;
using CoreVokiCreationService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;

public class DraftVokisConfigurations : IEntityTypeConfiguration<DraftVoki>
{
    public void Configure(EntityTypeBuilder<DraftVoki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion()
            .HasColumnName("Id");

        builder
            .Property(x => x.Name)
            .HasConversion<VokiNameConverter>();

        builder
            .Property(x => x.Cover)
            .HasConversion<VokiCoverKeyConverter>();

        builder
            .Property(x => x.PrimaryAuthorId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion()
            .HasColumnName("PrimaryAuthorId");

        builder
            .Property(x => x.CoAuthorIds)
            .HasGuidBasedIdsImmutableHashSetConversion()
            .HasColumnName("CoAuthorIds");

        builder
            .ComplexProperty(x => x.ExpectedManagers, e => {
                e.Property(s=>s.UserIdsToBecomeManagers)
                    .HasConversion<VokiManagersIdsSetConverter>()
                    .HasColumnName("expectedManagers_UserIdsToBecomeManagers");

                e.Property(s=>s.MakeAllCoAuthorsManagers)
                    .HasColumnName("expectedManagers_MakeAllCoAuthorsManagers");
            });

        builder
            .Property(x => x.InvitedForCoAuthorUserIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property(x => x.CreationDate)
            .HasColumnName("CreationDate");
    }
}