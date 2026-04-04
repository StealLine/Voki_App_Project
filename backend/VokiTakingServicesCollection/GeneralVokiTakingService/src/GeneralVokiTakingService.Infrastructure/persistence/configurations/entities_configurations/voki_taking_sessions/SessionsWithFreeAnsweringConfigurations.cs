using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.free_answering;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.voki_taking_sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.voki_taking_sessions;

public class SessionsWithFreeAnsweringConfigurations : IEntityTypeConfiguration<SessionWithFreeAnswering>
{
    public void Configure(EntityTypeBuilder<SessionWithFreeAnswering> builder)
    {
        builder
            .Property<SessionWithFreeAnsweringSavedState>("_savedState")
            .HasConversion<SessionWithFreeAnsweringSavedStateConverter>()
            .HasColumnName("SavedState");
    }
}