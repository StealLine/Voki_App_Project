using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

public abstract partial record BaseQuestionAnswer(
    AnswerOrderInQuestion Order,
    AnswerRelatedResultIdsSet RelatedResultIds
)
{
    [Pure]
    public BaseQuestionAnswer RemoveRelatedResult(GeneralVokiResultId id) =>
        this with { RelatedResultIds = RelatedResultIds.Remove(id) };
}

public interface IAnswerWithStorageKey
{
    public bool IsForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId);
}