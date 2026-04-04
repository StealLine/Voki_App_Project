using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

public record class VokiQuestionBriefDataResponse(
    string Id,
    string Text,
    string[] Images,
    GeneralVokiQuestionContentType ContentType,
    ushort OrderInVoki,
    bool ShuffleAnswers,
    bool IsMultipleChoice
)
{
    public static VokiQuestionBriefDataResponse Create(VokiQuestion question) => new(
        question.Id.ToString(),
        question.Text.ToString(),
        question.ImageSet.Keys.Select(k => k.ToString()).ToArray(),
        question.Content.Type,
        question.OrderInVoki.Value,
        ShuffleAnswers: question.ShuffleAnswers,
        question.AnswersCountLimit.IsMultipleChoice
    );
}