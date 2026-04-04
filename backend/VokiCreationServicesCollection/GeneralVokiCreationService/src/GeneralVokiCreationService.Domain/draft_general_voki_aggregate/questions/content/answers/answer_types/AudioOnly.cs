using SharedKernel.common.vokis.general_vokis;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

public abstract partial record BaseQuestionAnswer
{
    public sealed record AudioOnly(
        GeneralVokiAnswerAudioKey Audio,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    ) : BaseQuestionAnswer(Order, RelatedResultIds), IAnswerWithStorageKey
    {
        public bool IsForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
            Audio.IsWithIds(vokiId, questionId);
    }
}