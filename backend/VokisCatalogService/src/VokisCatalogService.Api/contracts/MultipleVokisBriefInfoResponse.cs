using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts;

internal record class MultipleVokisBriefInfoResponse(
    VokiBriefInfoResponse[] Vokis
)
{
    public static MultipleVokisBriefInfoResponse Create(IEnumerable<Voki> vokis) => new(
        vokis.Select(VokiBriefInfoResponse.Create).ToArray()
    );
}