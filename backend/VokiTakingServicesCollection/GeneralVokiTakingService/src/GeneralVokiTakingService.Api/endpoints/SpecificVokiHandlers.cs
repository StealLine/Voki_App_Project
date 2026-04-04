using GeneralVokiTakingService.Api.contracts.voki_taking;
using GeneralVokiTakingService.Api.contracts.voki_taking.free_answering;
using GeneralVokiTakingService.Api.contracts.voki_taking.free_answering.save_state;
using GeneralVokiTakingService.Api.contracts.voki_taking.sequential_answering;
using GeneralVokiTakingService.Api.contracts.voki_taking.shared;
using GeneralVokiTakingService.Api.contracts.voki_taking.shared.continue_taking;
using GeneralVokiTakingService.Api.contracts.voki_taking.shared.start;
using GeneralVokiTakingService.Application.general_vokis.commands;
using GeneralVokiTakingService.Application.general_vokis.commands.free_answering_voki_taking;
using GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;
using GeneralVokiTakingService.Application.general_vokis.queries;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.free_answering;

namespace GeneralVokiTakingService.Api.endpoints;

internal class SpecificVokiHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/does-user-have-unfinished-session", DoesUserHaveUnfinishedTakingSessionForThisVoki);

        group.MapPost("/start-taking", StartVokiTaking)
            .WithRequestValidation<StartVokiTakingRequest>();

        group.MapPost("/continue-taking", ContinueVokiTaking)
            .WithRequestValidation<ContinueVokiTakingRequest>();

        group.MapPost("/free-answering/save-current-state", SaveCurrentFreeVokiTakingSessionState)
            .WithRequestValidation<SaveCurrentFreeVokiTakingSessionStateRequest>();

        group.MapPost("/free-answering/finish", FinishVokiTakingWithFreeAnswering)
            .WithRequestValidation<FinishVokiTakingWithFreeAnsweringRequest>();

        group.MapPost("/sequential-answering/finish", FinishVokiTakingWithSequentialAnswering)
            .WithRequestValidation<FinishVokiTakingWithSequentialAnsweringRequest>();

        group.MapPost("/sequential-answering/answer-question", AnswerQuestionForSequentialAnsweringSession)
            .WithRequestValidation<AnswerQuestionForSequentialAnsweringSessionRequest>();

        return group;
    }

    private static async Task<IResult> DoesUserHaveUnfinishedTakingSessionForThisVoki(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<CheckIfUserHasUnfinishedSessionForVokiQuery, CheckIfUserHasUnfinishedSessionForVokiQueryResult> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        CheckIfUserHasUnfinishedSessionForVokiQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<
            CheckIfUserHasUnfinishedSessionForVokiQueryResult, ExistingUnfinishedSessionCheckResponse
        >(result);
    }

    private static async Task<IResult> StartVokiTaking(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<StartVokiTakingCommand, IStartVokiTakingCommandResult> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<StartVokiTakingRequest>();

        StartVokiTakingCommand command = new(id, request.TerminateExistingUnfinishedSession);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<IStartVokiTakingCommandResult, StartVokiTakingResponse>(result);
    }

    private static async Task<IResult> ContinueVokiTaking(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<ContinueVokiTakingCommand, ContinueVokiTakingCommandResult> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<ContinueVokiTakingRequest>();


        ContinueVokiTakingCommand command = new(id, request.ParsedSessionId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<ContinueVokiTakingCommandResult, ContinueVokiTakingResponse>(result);
    }

    private static async Task<IResult> SaveCurrentFreeVokiTakingSessionState(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<SaveCurrentFreeVokiTakingSessionStateCommand, SessionWithFreeAnsweringSavedState> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveCurrentFreeVokiTakingSessionStateRequest>();

        SaveCurrentFreeVokiTakingSessionStateCommand command = new(vokiId, request.ParsedSessionId, request.ParsedChosenAnswers);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<SessionWithFreeAnsweringSavedState, SaveFreeVokiTakingSessionStateResponse>(result);
    }

    private static async Task<IResult> FinishVokiTakingWithFreeAnswering(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<FinishVokiTakingWithFreeAnsweringCommand, GeneralVokiResultId> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<FinishVokiTakingWithFreeAnsweringRequest>();

        FinishVokiTakingWithFreeAnsweringCommand command = new(
            id, request.ParsedSessionId, request.ParsedChosenAnswers,
            request.ParsedSessionStartTime, request.ClientSessionFinishTime
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (resId) => Results.Json(
            new VokiTakingFinishedResponse(resId.ToString())
        ));
    }

    private static async Task<IResult> FinishVokiTakingWithSequentialAnswering(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<FinishVokiTakingWithSequentialAnsweringCommand, GeneralVokiResultId> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<FinishVokiTakingWithSequentialAnsweringRequest>();


        FinishVokiTakingWithSequentialAnsweringCommand command = new(
            vokiId, request.ParsedSessionId, request.ParsedSessionStartTime,
            ClientSessionFinishTime: request.ClientSessionFinishTime,
            request.ParsedLastQuestionId, request.ParsedLastQuestionOrder,
            request.ParsedQuestionChosenAnswers, request.ParsedLastQuestionShownAt,
            LastQuestionClientAnsweredAt: request.ClientLastQuestionAnsweredAt
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (resId) => Results.Json(
            new VokiTakingFinishedResponse(resId.ToString())
        ));
    }

    private static async Task<IResult> AnswerQuestionForSequentialAnsweringSession(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AnswerQuestionInSequentialAnsweringVokiTakingCommand,
            AnswerQuestionInSequentialAnsweringVokiTakingCommandResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<AnswerQuestionForSequentialAnsweringSessionRequest>();

        AnswerQuestionInSequentialAnsweringVokiTakingCommand command = new(
            vokiId, request.ParsedSessionId, request.ParsedQuestionId,
            ShownAt: request.ParsedShownAt,
            ClientQuestionAnsweredAt: request.ClientQuestionAnsweredAt,
            request.ParsedQuestionOrder,
            request.ParsedChosenAnswers
        );
        var result = await handler.Handle(command, ct);

        return CustomResults
            .FromErrOrToJson<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult,
                SequentialAnswerSessionNextQuestionResponse>(result);
    }
}