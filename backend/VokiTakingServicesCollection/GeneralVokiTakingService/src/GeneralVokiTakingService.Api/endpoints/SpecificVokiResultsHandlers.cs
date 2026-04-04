using GeneralVokiTakingService.Api.contracts.view_results;
using GeneralVokiTakingService.Api.extensions;
using GeneralVokiTakingService.Application.general_vokis.queries;

namespace GeneralVokiTakingService.Api.endpoints;

internal class SpecificVokiResultsHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/results");

        group.MapGet("/{resultId}", ViewSpecificResult);

        group.MapGet("/all", ViewAllVokiResults);

        group.MapGet("/received", ViewVokiReceivedResults);
        
        return group;
    }

    private static async Task<IResult> ViewSpecificResult(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ViewVokiResultQuery, ViewVokiResultQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();

        ViewVokiResultQuery query = new(vokiId, resultId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ViewVokiResultQueryResult, ViewSingleResultResponse>(result);
    }

    private static async Task<IResult> ViewAllVokiResults(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ViewAllVokiResultsQuery, ViewAllVokiResultsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ViewAllVokiResultsQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ViewAllVokiResultsQueryResult, ViewAllVokiResultsResponse>(result);
    }

    private static async Task<IResult> ViewVokiReceivedResults(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<VokiReceivedResultsQuery, VokiReceivedResultsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        VokiReceivedResultsQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiReceivedResultsQueryResult, ViewReceivedResultsResponse>(result);
    }
}