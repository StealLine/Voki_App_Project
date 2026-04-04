using VokisCatalogService.Api.contracts;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/taken-vokis", GetUserTakenVokis);
        
        return group;
    }

    private static async Task<IResult> GetUserTakenVokis(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListUserTakenVokisQuery,UserTakenVokisList> handler
    ) {
        ListUserTakenVokisQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserTakenVokisList, UserTakenVokisResponse>(result);
    }
}