using ApiShared;
using ApplicationShared.messaging;
using VokiCommentsService.Api.contracts;
using VokiCommentsService.Application.app_users.queries;
using VokiCommentsService.Application.common.repositories;

namespace VokiCommentsService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/commented-vokis", GetUserCommentedVokis);

        return group;
    }

    private static async Task<IResult> GetUserCommentedVokis(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListUserCommentedVokiIdsQuery, VokiIdWithLastCommentedDateDto[]> handler
    ) {
        ListUserCommentedVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiIdWithLastCommentedDateDto[], UserCommentedVokiIdsResponse>(result);
    }
}