using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.voki_taking_sessions;

public class SessionsWithSequentialAnsweringConfigurations : IEntityTypeConfiguration<SessionWithSequentialAnswering>
{
    public void Configure(EntityTypeBuilder<SessionWithSequentialAnswering> builder) {
        builder
            .Property<ImmutableArray<SequentialTakingAnsweredQuestion>>("_answered")
            .HasField("_answered")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasSequentialTakingAnsweredQuestionsConversion()
            .HasColumnName("AnsweredQuestions");
    }
}