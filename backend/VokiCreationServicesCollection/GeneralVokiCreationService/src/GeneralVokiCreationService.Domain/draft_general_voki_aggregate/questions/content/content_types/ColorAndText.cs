using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;


public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record ColorAndText(
        QuestionAnswersList<BaseQuestionAnswer.ColorAndText> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiQuestionContentType Type => GeneralVokiQuestionContentType.ColorAndText;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;


        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new ColorAndText(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.ColorAndText)a.RemoveRelatedResult(resultId))
        );

        public override IQuestionContentIntegrationEventDto ToIntegrationEventDto() => new ColorAndTextQuestionIntegrationEventDto(Answers
            .Select(a => new ColorAndTextQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Text: a.Text.ToString(),
                Color: a.Color.ToString()
            ))
            .ToArray()
        );

        public static ColorAndText Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.ColorAndText>.Empty()
        );
    }

}