using SharedKernel.common.vokis;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts;

internal sealed record class VokiBriefInfoResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] ManagerIds,
    bool HasMatureContent,
    Language Language,
    bool SignedInOnlyTaking,
    DateTime PublicationDate
)
{
    public static VokiBriefInfoResponse Create(Voki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.Cover.ToString(),
        v.PrimaryAuthorId.ToString(),
        CoAuthorIds: v.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        ManagerIds: v.ManagersSet.ToArray().Select(id => id.ToString()).ToArray(),
        v.Details.HasMatureContent,
        v.Details.Language,
        v.InteractionSettings.SignedInOnlyTaking,
        v.PublicationDate
    );
}