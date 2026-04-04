using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.extensions;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.voki_taking_sessions;

public class BaseVokiTakingSessionsConfigurations : IEntityTypeConfiguration<BaseVokiTakingSession>
{
    public void Configure(EntityTypeBuilder<BaseVokiTakingSession> builder) {
        builder.UseTptMappingStrategy();

        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.VokiId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.VokiTaker)
            .ValueGeneratedNever()
            .HasNullableGuidBasedIdConversion();

        builder.Property(x => x.StartTime);

        builder.Ignore(x => x.TotalQuestionsCount);
        builder
            .Property<ImmutableArray<TakingSessionExpectedQuestion>>("Questions")
            .HasSessionExpectedQuestionsConversion();
    }
}