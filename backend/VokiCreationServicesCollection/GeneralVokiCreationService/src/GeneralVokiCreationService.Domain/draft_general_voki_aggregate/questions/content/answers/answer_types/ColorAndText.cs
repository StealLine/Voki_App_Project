using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

public abstract partial record BaseQuestionAnswer
{
    public sealed record ColorAndText(
        GeneralVokiAnswerText Text,
        HexColor Color,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    ) : BaseQuestionAnswer(Order, RelatedResultIds)
    {
    }
}