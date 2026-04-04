using GeneralVokiTakingService.Application.general_vokis.queries;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record class ViewSingleResultResponse(
    string Id,
    string Name,
    string Text,
    string? Image,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName,
    uint ResultsCount,
    bool HasUserTakenThisVoki
) : ICreatableResponse<ViewVokiResultQueryResult>
{
    public static ICreatableResponse<ViewVokiResultQueryResult> Create(ViewVokiResultQueryResult res) =>
        new ViewSingleResultResponse(
            res.Result.Id.ToString(),
            res.Result.Name,
            res.Result.Text,
            res.Result.Image?.ToString() ?? null,
            res.ResultsVisibility,
            res.VokiName.ToString(),
            res.TotalResultsCount,
            res.HasUserTakenThisVoki
        );
}