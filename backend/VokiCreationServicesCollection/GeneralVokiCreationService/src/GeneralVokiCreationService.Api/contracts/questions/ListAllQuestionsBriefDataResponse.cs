using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.questions;

public record class ListAllQuestionsBriefDataResponse(
    VokiQuestionBriefDataResponse[] Questions
)
{
    public static ListAllQuestionsBriefDataResponse Create(ImmutableArray<VokiQuestion> question) => new(
        question
            .Select(VokiQuestionBriefDataResponse.Create)
            .OrderBy(a => a.OrderInVoki)
            .ToArray()
    );
}