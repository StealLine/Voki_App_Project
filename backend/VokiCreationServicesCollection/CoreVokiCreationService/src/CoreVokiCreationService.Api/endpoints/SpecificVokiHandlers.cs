using CoreVokiCreationService.Api.contracts;
using CoreVokiCreationService.Api.contracts.managers;
using CoreVokiCreationService.Api.contracts.vokis_brief_info;
using CoreVokiCreationService.Application.draft_vokis.commands;
using CoreVokiCreationService.Application.draft_vokis.commands.invites;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.endpoints;

internal class SpecificVokiHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/brief-info", GetVokiBriefInfo);
        group.MapGet("/authors-info", GetVokiAuthorsInfo);

        group.MapDelete("/leave-voki-creation", LeaveVokiCreation);

        group.MapDelete("/drop-co-author", DropCoAuthor)
            .WithRequestValidation<VokiCoAuthorActionRequest>();
        group.MapPost("/invite-co-authors", InviteCoAuthors)
            .WithRequestValidation<InviteCoAuthorsRequest>();
        group.MapDelete("/cancel-co-author-invite", CancelCoAuthorInvite)
            .WithRequestValidation<VokiCoAuthorActionRequest>();

        group.MapPatch("/accept-co-author-invite", AcceptCoAuthorInvite);
        group.MapPatch("/decline-co-author-invite", DeclineCoAuthorInvite);

        group.MapPatch("/update-expected-managers", UpdateExpectedManagers)
            .WithRequestValidation<UpdateVokiExpectedManagersRequest>();
        
        return group;

    }

    private static async Task<IResult> GetVokiBriefInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetVokiQuery, DraftVoki> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiBriefInfoResponse>(result);
    }

    private static async Task<IResult> GetVokiAuthorsInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetVokiQuery, DraftVoki> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiAuthorsInfoResponse>(result);
    }

    private static async Task<IResult> InviteCoAuthors(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<InviteCoAuthorCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<InviteCoAuthorsRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        InviteCoAuthorCommand command = new(vokiId, request.ParsedUserIds);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiCoAuthorsWithInvitedResponse>(result);
    }

    private static async Task<IResult> LeaveVokiCreation(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<LeaveVokiCreationCommand> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        LeaveVokiCreationCommand command = new(vokiId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }

    private static async Task<IResult> DropCoAuthor(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<DropCoAuthorCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<VokiCoAuthorActionRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        DropCoAuthorCommand command = new(vokiId, request.ParsedUserId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiCoAuthorsWithInvitedResponse>(result);
    }

    private static async Task<IResult> CancelCoAuthorInvite(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<CancelCoAuthorInviteCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<VokiCoAuthorActionRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        CancelCoAuthorInviteCommand command = new(vokiId, request.ParsedUserId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiCoAuthorsWithInvitedResponse>(result);
    }

    private static async Task<IResult> AcceptCoAuthorInvite(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<AcceptCoAuthorInviteCommand> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        AcceptCoAuthorInviteCommand command = new(vokiId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }

    private static async Task<IResult> DeclineCoAuthorInvite(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<DeclineCoAuthorInviteCommand> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        DeclineCoAuthorInviteCommand command = new(vokiId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }

    private static async Task<IResult> UpdateExpectedManagers(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateExpectedManagersCommand, VokiExpectedManagersSetting> handler
    ) {
        var request = httpContext.GetValidatedRequest<UpdateVokiExpectedManagersRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        UpdateExpectedManagersCommand command = new(vokiId, request.ParsedSetting);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiExpectedManagersSetting, VokiExpectedManagersSettingResponse>(result);
    }
}