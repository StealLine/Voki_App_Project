using GeneralVokiCreationService.Api.contracts.results;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.results;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.endpoints;

internal class SpecificVokiResultsHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/results/{resultId}/");

        group.MapPut("/update", UpdateVokiResult)
            .WithRequestValidation<UpdateVokiResultRequest>();

        group.MapDelete("/delete", DeleteVokiResult);

        return group;
    }

    private static async Task<IResult> UpdateVokiResult(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateVokiResultCommand, VokiResult> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiResultRequest>();

        UpdateVokiResultCommand command = new(id, resultId, request.ParsedName, request.ParsedText, request.NewImage);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiResult, VokiResultDataResponse>(result);
    }

    private static async Task<IResult> DeleteVokiResult(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<DeleteVokiResultCommand> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();

        DeleteVokiResultCommand command = new(id, resultId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, CustomResults.Deleted);
    }
}