using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Api.contracts;
using VokiCreationServicesLib.Api.contracts.update_requests;
using VokiCreationServicesLib.Api.contracts.voki_publishing;
using VokiCreationServicesLib.Application.draft_vokis.commands;
using VokiCreationServicesLib.Application.draft_vokis.queries;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;

namespace VokiCreationServicesLib.Api;

public abstract class BaseSpecificVokiHandlers
{
    protected RouteGroupBuilder CreateGroupWithBaseEndpoint(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/ensure-exists", CheckIfVokiExists);

        group.MapGet("/voki-name", GetVokiName);

        group.MapPatch("/set-cover-to-default", SetVokiCoverToDefault);
        group.MapPatch("/update-cover", UpdateVokiCover)
            .WithRequestValidation<UpdateVokiCoverRequest>();

        group.MapPatch("/update-name", UpdateVokiName)
            .WithRequestValidation<UpdateVokiNameRequest>();

        group.MapPatch("/update-details", UpdateVokiDetails)
            .WithRequestValidation<UpdateVokiDetailsRequest>();

        group.MapPatch("/update-tags", UpdateVokiTags)
            .WithRequestValidation<UpdateVokiTagsRequest>();

        group.MapGet("/publishing-data", GetVokiPublishingData);

        group.MapPost("/publish-with-no-issues", PublishVokiWithNoIssuesHandler)
            .WithRequestValidation<PublishVokiRequest>();
        group.MapPost("/publish-with-warnings-ignored", PublishVokiWithWarningsIgnoredHandler)
            .WithRequestValidation<PublishVokiRequest>();


        return group;
    }

    private static async Task<IResult> CheckIfVokiExists(
        CancellationToken ct, HttpContext httpContext,
        [FromServices] IQueryHandler<EnsureVokiExistsQuery, VokiId> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        var result = await handler.Handle(new EnsureVokiExistsQuery(id), ct);

        return CustomResults.FromErrOr(result, _ => Results.Ok());
    }

    private static async Task<IResult> GetVokiName(
        CancellationToken ct, HttpContext httpContext,
        [FromServices] IQueryHandler<GetVokiNameQuery, VokiName> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var result = await handler.Handle(new GetVokiNameQuery(id), ct);

        return CustomResults.FromErrOrToJson<VokiName, VokiNameResponse>(result);
    }


    private static async Task<IResult> UpdateVokiName(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiNameCommand, VokiName> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var req = httpContext.GetValidatedRequest<UpdateVokiNameRequest>();

        var result = await handler.Handle(new UpdateVokiNameCommand(id, req.ParsedVokiName), ct);
        return CustomResults.FromErrOr(result, name => Results.Json(new { NewVokiName = name.ToString() }));
    }


    private static async Task<IResult> SetVokiCoverToDefault(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<SetVokiCoverToDefaultCommand, VokiCoverKey> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var result = await handler.Handle(new SetVokiCoverToDefaultCommand(id), ct);

        return CustomResults.FromErrOr(result, key => Results.Json(new { NewCover = key.ToString() }));
    }


    private static async Task<IResult> UpdateVokiCover(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiCoverCommand, VokiCoverKey> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var req = httpContext.GetValidatedRequest<UpdateVokiCoverRequest>();

        var result = await handler.Handle(new UpdateVokiCoverCommand(id, req.ParsedCover), ct);
        return CustomResults.FromErrOr(result, key => Results.Json(new { NewCover = key.ToString() }));
    }


    private static async Task<IResult> UpdateVokiDetails(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiDetailsCommand, VokiDetails> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var req = httpContext.GetValidatedRequest<UpdateVokiDetailsRequest>();

        var result = await handler.Handle(new UpdateVokiDetailsCommand(id, req.ParsedDetails), ct);
        return CustomResults.FromErrOrToJson<VokiDetails, VokiDetailsResponse>(result);
    }


    private static async Task<IResult> UpdateVokiTags(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiTagsCommand, VokiTagsSet> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var req = httpContext.GetValidatedRequest<UpdateVokiTagsRequest>();

        var result = await handler.Handle(new UpdateVokiTagsCommand(id, req.ParsedTags), ct);

        return CustomResults.FromErrOr(result, tagsSet =>
            Results.Json(new { NewTags = tagsSet.Value.Select(t => t.ToString()).ToArray() }));
    }

    protected abstract Delegate GetVokiPublishingData { get; }
    protected abstract Delegate PublishVokiWithNoIssuesHandler { get; }
    protected abstract Delegate PublishVokiWithWarningsIgnoredHandler { get; }
}