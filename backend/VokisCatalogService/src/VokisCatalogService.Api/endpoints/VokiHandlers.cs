using VokisCatalogService.Api.contracts;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.endpoints;

internal class VokiHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/");

        group.MapGet("/all", ListAllVokis);
        group.MapGet("/list-user-voki-ids", ListUserVokiIds);
        group.MapPost("/brief-info", ListVokisBriefInfo)
            .WithRequestValidation<ListVokisBriefInfoRequest>();

        return group;
    }

    private static async Task<IResult> ListUserVokiIds(
        CancellationToken ct, IQueryHandler<ListIdsOfVokiAuthoredByUser, VokiId[]> handler
    ) {
        ListIdsOfVokiAuthoredByUser query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokiIds) => Results.Json(
            new { VokiIds = vokiIds.Select(v => v.ToString()).ToArray() }
        ));
    }

    private static async Task<IResult> ListAllVokis(
        CancellationToken ct, IQueryHandler<ListAllVokisQuery, Voki[]> handler
    ) {
        ListAllVokisQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokis) => Results.Json(
            MultipleVokisBriefInfoResponse.Create(vokis)
        ));
    }

    private static async Task<IResult> ListVokisBriefInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListVokisWithIdsQuery, Voki[]> handler
    ) {
        var request = httpContext.GetValidatedRequest<ListVokisBriefInfoRequest>();

        ListVokisWithIdsQuery withIdsQuery = new(request.ParsedVokiIds);
        var result = await handler.Handle(withIdsQuery, ct);

        return CustomResults.FromErrOr(result, (vokis) => Results.Json(
            MultipleVokisBriefInfoResponse.Create(vokis)
        ));
    }
}