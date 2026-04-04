using VokisCatalogService.Api.contracts;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.endpoints;

internal class SpecificVokiHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/overview", GetVokiOverviewInfo);
        
        return group;
    }

    private static async Task<IResult> GetVokiOverviewInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuery, Voki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<Voki, VokiOverviewResponse>(result);
    }
}