using ApiShared.extensions;
using VokiRatingsService.Api.contracts.manage_voki;
using VokiRatingsService.Application.vokis.commands;
using VokiRatingsService.Application.vokis.queries;

namespace VokiRatingsService.Api.endpoints;

internal class ManageVokiEndpoints : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/manage");

        group.MapGet("/overview", ManageVokiRatingsOverview);
        group.MapPost("/take-snapshot", TakeAndRetrieveVokiRatingsSnapshot);
        
        return group;
    }

    private static async Task<IResult> ManageVokiRatingsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ManageVokiRatingsOverviewQuery, ManageVokiRatingsOverviewQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ManageVokiRatingsOverviewQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ManageVokiRatingsOverviewQueryResult, ManageVokiRatingsOverviewResponse>(result);
    }
    private static async Task<IResult> TakeAndRetrieveVokiRatingsSnapshot(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<TakeVokiRatingsSnapshotCommand, ManageVokiRatingsOverviewQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        TakeVokiRatingsSnapshotCommand command = new(vokiId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<ManageVokiRatingsOverviewQueryResult, ManageVokiRatingsOverviewResponse>(result);
    }
}