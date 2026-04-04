using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;

public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record TextOnly(
        QuestionAnswersList<BaseQuestionAnswer.TextOnly> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiQuestionContentType Type => GeneralVokiQuestionContentType.TextOnly;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;


        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new TextOnly(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.TextOnly)a.RemoveRelatedResult(resultId))
        );

        public override IQuestionContentIntegrationEventDto ToIntegrationEventDto() => new TextOnlyQuestionIntegrationEventDto(Answers
            .Select(a => new TextOnlyQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Text: a.Text.ToString()
            ))
            .ToArray()
        );

        public static TextOnly Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.TextOnly>.Empty()
        );
    }
}