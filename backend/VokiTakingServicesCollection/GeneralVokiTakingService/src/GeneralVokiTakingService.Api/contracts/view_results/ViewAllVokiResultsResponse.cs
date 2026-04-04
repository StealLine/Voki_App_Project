using GeneralVokiTakingService.Application.general_vokis.queries;
using GeneralVokiTakingService.Domain.common.dtos;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record class ViewAllVokiResultsResponse(
    VokiResultPreviewWithPercentageResponse[] Results,
    bool ShowResultsDistribution,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName,
    bool HasUserTakenThisVoki
) : ICreatableResponse<ViewAllVokiResultsQueryResult>
{
    public static ICreatableResponse<ViewAllVokiResultsQueryResult> Create(
        ViewAllVokiResultsQueryResult queryResult
    ) => new ViewAllVokiResultsResponse(
        queryResult.Results.Select(VokiResultPreviewWithPercentageResponse.Create).ToArray(),
        queryResult.ShowResultsDistribution,
        queryResult.ResultsVisibility,
        queryResult.VokiName.ToString(),
        queryResult.HasUserTaken
    );
}

public record class VokiResultPreviewWithPercentageResponse(
    string Id,
    string Name,
    string? Image,
    double DistributionPercent
)
{
    public static VokiResultPreviewWithPercentageResponse Create(VokiResultWithDistributionPercent resWithPercentage) => new(
        resWithPercentage.Result.Id.ToString(),
        resWithPercentage.Result.Name,
        resWithPercentage.Result.Image?.ToString() ?? null,
        resWithPercentage.DistributionPercent
    );
}