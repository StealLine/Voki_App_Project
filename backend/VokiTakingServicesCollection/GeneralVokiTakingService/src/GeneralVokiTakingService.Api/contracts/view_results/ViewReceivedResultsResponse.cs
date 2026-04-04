using GeneralVokiTakingService.Application.general_vokis.queries;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record ViewReceivedResultsResponse(
    ResultPreviewWithUserTakingsResponse[] Results,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName,
    uint ResultsCount
) : ICreatableResponse<VokiReceivedResultsQueryResult>
{
    public static ICreatableResponse<VokiReceivedResultsQueryResult> Create(VokiReceivedResultsQueryResult queryResult) =>
        new ViewReceivedResultsResponse(
            queryResult.ResultsWithTakings.Select(
                (resultWithRecords) => ResultPreviewWithUserTakingsResponse.Create(
                    resultWithRecords.Result,
                    resultWithRecords.Records
                )
            ).ToArray(),
            queryResult.ResultsVisibility,
            queryResult.VokiName.ToString(),
            queryResult.TotalResultsCount
        );
}

public record ResultPreviewWithUserTakingsResponse(
    string Id,
    string Name,
    string? Image,
    VokiTakenRecordForResultResponse[] Takings
)
{
    public static ResultPreviewWithUserTakingsResponse Create(VokiResult result, GeneralVokiTakenRecord[] records) => new(
        result.Id.ToString(),
        result.Name,
        result.Image?.ToString() ?? null,
        records
            .Select(r => new VokiTakenRecordForResultResponse(
                r.Id.ToString(),
                r.StartTime,
                r.FinishTime
            ))
            .ToArray()
    );
}

public record VokiTakenRecordForResultResponse(string Id, DateTime Start, DateTime Finish);