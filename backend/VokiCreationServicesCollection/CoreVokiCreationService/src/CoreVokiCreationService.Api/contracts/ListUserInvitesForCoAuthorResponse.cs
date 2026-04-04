using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts;

public record ListUserInvitesForCoAuthorResponse(
    InviteForCoAuthorVokiPreview[] Invites
) : ICreatableResponse<DraftVoki[]>
{
    public static ICreatableResponse<DraftVoki[]> Create(DraftVoki[] vokis) => new ListUserInvitesForCoAuthorResponse(
        vokis.Select(InviteForCoAuthorVokiPreview.Create).ToArray()
    );
}

public record InviteForCoAuthorVokiPreview(
    string VokiId,
    string VokiName,
    string VokiCover,
    VokiType VokiType,
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] InvitedForCoAuthorUserIds,
    DateTime CreationDate
)
{
    public static InviteForCoAuthorVokiPreview Create(DraftVoki v) => new(
        v.Id.ToString(),
        v.Name.ToString(),
        v.Cover.ToString(),
        v.Type,
        v.PrimaryAuthorId.ToString(),
        v.CoAuthorIds.Select(a => a.ToString()).ToArray(),
        v.InvitedForCoAuthorUserIds.Select(a => a.ToString()).ToArray(),
        v.CreationDate
    );
}