using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;

public class VokiQuestionsConfigurations : IEntityTypeConfiguration<VokiQuestion>
{
    public void Configure(EntityTypeBuilder<VokiQuestion> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();
        
        builder
            .Property(x => x.Text)
            .HasConversion<VokiQuestionTextConverter>();

        builder
            .Property(x => x.ImageSet)
            .HasConversion<VokiQuestionImagesSetConverter>();

        builder
            .Property(x => x.Content)
            .HasConversion<VokiQuestionTypeSpecificContentConverter>();

        builder
            .Property(x => x.AnswersCountLimit)
            .HasConversion<QuestionAnswersCountLimitConverter>();
            
        builder
            .Property(x => x.OrderInVoki)
            .HasConversion<VokiQuestionOrderConverter>();
    }
}