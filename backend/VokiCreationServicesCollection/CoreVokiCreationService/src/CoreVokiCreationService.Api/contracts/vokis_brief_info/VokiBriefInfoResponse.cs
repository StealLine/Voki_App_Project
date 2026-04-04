using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts.vokis_brief_info;

internal sealed record class VokiBriefInfoResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds
) : ICreatableResponse<DraftVoki>
{
    public static ICreatableResponse<DraftVoki> Create(DraftVoki v) => FromVoki(v);

    public static VokiBriefInfoResponse FromVoki(DraftVoki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.Cover.ToString(),
        v.PrimaryAuthorId.ToString(),
        v.CoAuthorIds.Select(id => id.ToString()).ToArray()
    );
}