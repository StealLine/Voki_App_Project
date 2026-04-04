using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.contracts;

public record class VokiCoAuthorsWithInvitedResponse(
    string[] CoAuthorIds,
    string[] InvitedForCoAuthorUserIds
) : ICreatableResponse<DraftVoki>
{
    public static ICreatableResponse<DraftVoki> Create(DraftVoki voki) => new VokiCoAuthorsWithInvitedResponse(
        voki.CoAuthorIds.Select(a => a.ToString()).ToArray(),
        voki.InvitedForCoAuthorUserIds.Select(a => a.ToString()).ToArray()
    );
}