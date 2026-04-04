using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

using SharedKernel.integration_events.voki_publishing;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;


public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record ImageOnly(
        QuestionAnswersList<BaseQuestionAnswer.ImageOnly> Answers
    ) : BaseQuestionTypeSpecificContent, IContentWithStorageKeys
    {
        public override GeneralVokiQuestionContentType Type => GeneralVokiQuestionContentType.ImageOnly;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;


        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new ImageOnly(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.ImageOnly)a.RemoveRelatedResult(resultId))
        );

        public override IQuestionContentIntegrationEventDto ToIntegrationEventDto() => new ImageOnlyQuestionIntegrationEventDto(Answers
            .Select(a => new ImageOnlyQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Image: a.Image.ToString()
            ))
            .ToArray()
        );

        public static ImageOnly Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.ImageOnly>.Empty()
        );

        public bool IsAllForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
            Answers.All(a => a.IsForCorrectVokiQuestion(vokiId, questionId));
    }

}