using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.extensions;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations;

public class GeneralVokiTakenRecordsConfigurations : IEntityTypeConfiguration<GeneralVokiTakenRecord>
{
    public void Configure(EntityTypeBuilder<GeneralVokiTakenRecord> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.TakenVokiId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.VokiTakerId)
            .ValueGeneratedNever()
            .HasNullableGuidBasedIdConversion();

        builder.Property(x => x.StartTime);
        builder.Property(x => x.FinishTime);

        builder
            .Property(x => x.ReceivedResultId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.QuestionDetails)
            .HasVokiTakenQuestionDetailsArrayConversion();

        builder.Property(x => x.WasWithSequentialAnswering);
    }
}