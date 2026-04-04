using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;

public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record AudioAndText(
        QuestionAnswersList<BaseQuestionAnswer.AudioAndText> Answers
    )
        : BaseQuestionTypeSpecificContent, IContentWithStorageKeys
    {
        public override GeneralVokiQuestionContentType Type => GeneralVokiQuestionContentType.AudioAndText;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;

        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new AudioAndText(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.AudioAndText)a.RemoveRelatedResult(resultId))
        );

        public override IQuestionContentIntegrationEventDto ToIntegrationEventDto() =>
            new AudioAndTextQuestionIntegrationEventDto(Answers
                .Select(a => new AudioAndTextQuestionIntegrationEventDto.Answer(
                    Id: GeneralVokiAnswerId.CreateNew(),
                    Order: a.Order.Value,
                    RelatedResultIds: a.RelatedResultIds.ToArray(),
                    Text: a.Text.ToString(),
                    Audio: a.Audio.ToString()
                ))
                .ToArray()
            );

        public static AudioAndText Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.AudioAndText>.Empty()
        );

        public bool IsAllForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
            Answers.All(a => a.IsForCorrectVokiQuestion(vokiId, questionId));
    }
}