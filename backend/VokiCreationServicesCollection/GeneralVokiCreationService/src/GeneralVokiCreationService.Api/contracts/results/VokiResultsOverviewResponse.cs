using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.results;

public record class VokiResultsOverviewResponse(
    VokiResultDataResponse[] Results,
    int MaxVokiResultsCount
) : ICreatableResponse<ImmutableArray<VokiResult>>
{
    public static ICreatableResponse<ImmutableArray<VokiResult>> Create(ImmutableArray<VokiResult> results) =>
        new VokiResultsOverviewResponse(
            results
                .OrderBy(r => r.CreationDate)
                .Select(VokiResultDataResponse.FromResult)
                .ToArray(),
            DraftGeneralVoki.MaxResultsCount
        );
}