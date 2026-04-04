using VokiRatingsService.Api.contracts;
using VokiRatingsService.Application.app_users.queries;
using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/rated-vokis", GetUserRatedVokis);
        
        return group;
    }

    private static async Task<IResult> GetUserRatedVokis(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListUserRatedVokisQuery, VokiIdWithCurrentRatingDto[]> handler
    ) {
        ListUserRatedVokisQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiIdWithCurrentRatingDto[], UserRatedVokiIdsResponse>(result);
    }
}