using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Api.contracts.results;

public record VokiResultsIdToNameResponse(
    Dictionary<string, string> ResultsIdsToName
) : ICreatableResponse<ImmutableDictionary<GeneralVokiResultId, VokiResultName>>
{
    public static ICreatableResponse<ImmutableDictionary<GeneralVokiResultId, VokiResultName>> Create(
        ImmutableDictionary<GeneralVokiResultId, VokiResultName> results
    ) => new VokiResultsIdToNameResponse(
        results.ToDictionary(
            kvp => kvp.Key.ToString(),
            kvp => kvp.Value.ToString()
        )
    );
}