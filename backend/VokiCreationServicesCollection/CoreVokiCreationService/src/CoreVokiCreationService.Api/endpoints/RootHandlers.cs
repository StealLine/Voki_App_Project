using CoreVokiCreationService.Api.contracts;
using CoreVokiCreationService.Api.contracts.init_new_voki;
using CoreVokiCreationService.Application.draft_vokis.commands;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/list-user-voki-ids", ListUserVokiIds);
        group.MapPost("/initialize-new-voki", InitializeNewVoki)
            .WithRequestValidation<InitializeNewVokiRequest>();
        group.MapGet("/list-invites", ListUserInvites);
        
        return group;
    }

    private static async Task<IResult> ListUserVokiIds(
        CancellationToken ct, IQueryHandler<ListUserVokiIdsQuery, ImmutableArray<VokiId>> handler
    ) {
        ListUserVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokiIds) => Results.Json(
            new { VokiIds = vokiIds.Select(v => v.ToString()).ToArray() }
        ));
    }

    private static async Task<IResult> InitializeNewVoki(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<InitializeNewVokiCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<InitializeNewVokiRequest>();

        InitializeNewVokiCommand command = new(request.VokiType, request.ParseVokiName);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, NewVokiInitializedResponse>(result);
    }

    private static async Task<IResult> ListUserInvites(
        CancellationToken ct, IQueryHandler<ListVokisUserInvitedForCoAuthorQuery, DraftVoki[]> handler
    ) {
        ListVokisUserInvitedForCoAuthorQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<DraftVoki[], ListUserInvitesForCoAuthorResponse>(result);
    }
}