using GeneralVokiCreationService.Api.contracts.results;
using GeneralVokiCreationService.Application.draft_vokis.commands.results;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Application.draft_vokis.queries.results;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Api.endpoints;

internal class VokiResultsHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/results/");

        group.MapGet("/overview", GetVokiResultsOverview);
        group.MapPost("/add-new", AddNewResultToVoki)
            .WithRequestValidation<AddNewResultToVokiRequest>();
        group.MapGet("/ids-names", GetVokiResultsIdsWithNames);

        return group;
    }

    private static async Task<IResult> GetVokiResultsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListVokiResultsQuery, ImmutableArray<VokiResult>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        ListVokiResultsQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ImmutableArray<VokiResult>, VokiResultsOverviewResponse>(result);
    }

    private static async Task<IResult> AddNewResultToVoki(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AddNewResultToVokiCommand, ImmutableArray<VokiResult>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<AddNewResultToVokiRequest>();

        AddNewResultToVokiCommand command = new(id, request.ParsedName);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (results) => Results.Json(
            VokiResultsOverviewResponse.Create(results)
        ));
    }

    private static async Task<IResult> GetVokiResultsIdsWithNames(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiResultsIdToNameQuery, ImmutableDictionary<GeneralVokiResultId, VokiResultName>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiResultsIdToNameQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<
            ImmutableDictionary<GeneralVokiResultId, VokiResultName>,
            VokiResultsIdToNameResponse
        >(result);
    }
}