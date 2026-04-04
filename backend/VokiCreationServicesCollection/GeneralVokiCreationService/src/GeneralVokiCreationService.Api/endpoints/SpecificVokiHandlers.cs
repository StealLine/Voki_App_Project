using GeneralVokiCreationService.Api.contracts;
using GeneralVokiCreationService.Application.draft_vokis.commands;
using GeneralVokiCreationService.Application.draft_vokis.commands.publishing;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis.general_vokis;
using VokiCreationServicesLib.Api;
using VokiCreationServicesLib.Api.contracts;
using VokiCreationServicesLib.Api.contracts.voki_publishing;
using VokiCreationServicesLib.Application;

namespace GeneralVokiCreationService.Api.endpoints;

internal class SpecificVokiHandlers : BaseSpecificVokiHandlers, IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = base.CreateGroupWithBaseEndpoint(routeBuilder);

        group.MapGet("/main-info", GetVokiMainInfo);

        group.MapPatch("/update-interaction-settings", UpdateVokiInteractionSettings)
            .WithRequestValidation<UpdateInteractionSettingsRequest>();

        group.MapPatch("/update-voki-taking-process-settings", UpdateVokiTakingProcessSettings)
            .WithRequestValidation<UpdateVokiTakingProcessSettingsRequest>();

        return group;
    }
 

    private static async Task<IResult> GetVokiMainInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        var result = await handler.Handle(new GetVokiQuery(id), ct);

        return CustomResults.FromErrOrToJson<DraftGeneralVoki, VokiMainInfoResponse>(result);
    }

    private static async Task<IResult> UpdateVokiInteractionSettings(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiInteractionSettingsCommand, GeneralVokiInteractionSettings> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var req = httpContext.GetValidatedRequest<UpdateInteractionSettingsRequest>();

        var result = await handler.Handle(new UpdateVokiInteractionSettingsCommand(id, req.ParsedSettings), ct);
        return CustomResults.FromErrOrToJson<GeneralVokiInteractionSettings, VokiInteractionSettingsResponse>(result);
    }

    private static async Task<IResult> UpdateVokiTakingProcessSettings(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiTakingProcessSettingsCommand, VokiTakingProcessSettings> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiTakingProcessSettingsRequest>();

        var result = await handler.Handle(
            new UpdateVokiTakingProcessSettingsCommand(id, request.ParsedSettings),
            ct
        );
        return CustomResults.FromErrOr(result, settings => Results.Json(settings));
    }

    protected override Delegate GetVokiPublishingData => async (
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiPublishingDataQuery, GetVokiPublishingDataQueryResult> handler
    ) => {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var result = await handler.Handle(new GetVokiPublishingDataQuery(id), ct);

        return CustomResults.FromErrOrToJson<GetVokiPublishingDataQueryResult, VokiPublishingDataResponse>(result);
    };

    protected override Delegate PublishVokiWithNoIssuesHandler => async (
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<PublishVokiWithNoIssuesCommand, VokiSuccessfullyPublishedResult> handler
    ) => {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<PublishVokiRequest>();

        var result = await handler.Handle(
            new PublishVokiWithNoIssuesCommand(id, request.ParsedCoAuthorIds, request.ParsedUserIdsToBecomeManagers),
            ct
        );

        return CustomResults.FromErrOrToJson<VokiSuccessfullyPublishedResult, VokiSuccessfullyPublishedResponse>(result);
    };

    protected override Delegate PublishVokiWithWarningsIgnoredHandler => async (
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<PublishVokiWithWarningsIgnoredCommand, VokiSuccessfullyPublishedResult> handler
    ) => {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<PublishVokiRequest>();

        var result = await handler.Handle(
            new PublishVokiWithWarningsIgnoredCommand(id, request.ParsedCoAuthorIds, request.ParsedUserIdsToBecomeManagers),
            ct
        );

        return CustomResults.FromErrOrToJson<VokiSuccessfullyPublishedResult, VokiSuccessfullyPublishedResponse>(result);
    };
}