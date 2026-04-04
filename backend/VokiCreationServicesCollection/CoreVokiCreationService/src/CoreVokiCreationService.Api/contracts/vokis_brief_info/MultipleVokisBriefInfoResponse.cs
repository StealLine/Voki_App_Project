using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.contracts.vokis_brief_info;

internal record class MultipleVokisBriefInfoResponse(
    VokiBriefInfoResponse[] Vokis
) : ICreatableResponse<DraftVoki[]>
{
    public static ICreatableResponse<DraftVoki[]> Create(DraftVoki[] vokis) => new MultipleVokisBriefInfoResponse(
        vokis.Select(VokiBriefInfoResponse.FromVoki).ToArray()
    );
}