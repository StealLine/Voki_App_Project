using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;


public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record AudioOnly(
        QuestionAnswersList<BaseQuestionAnswer.AudioOnly> Answers
    )
        : BaseQuestionTypeSpecificContent, IContentWithStorageKeys
    {
        public override GeneralVokiQuestionContentType Type => GeneralVokiQuestionContentType.AudioOnly;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;

        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new AudioOnly(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.AudioOnly)a.RemoveRelatedResult(resultId))
        );

        public override IQuestionContentIntegrationEventDto ToIntegrationEventDto() => new AudioOnlyQuestionIntegrationEventDto(Answers
            .Select(a => new AudioOnlyQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Audio: a.Audio.ToString()
            ))
            .ToArray()
        );

        public static AudioOnly Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.AudioOnly>.Empty()
        );
        public bool IsAllForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
             Answers.All(a => a.IsForCorrectVokiQuestion(vokiId, questionId));
    }

}