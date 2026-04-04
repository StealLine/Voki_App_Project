using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record StartVokiTakingCommand(
    VokiId VokiId,
    bool TerminateExistingUnfinishedSession
) :
    ICommand<IStartVokiTakingCommandResult>;

internal sealed class StartVokiTakingCommandHandler : ICommandHandler<StartVokiTakingCommand, IStartVokiTakingCommandResult>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBaseTakingSessionsRepository _baseTakingSessionsRepository;
    private readonly ISessionsWithFreeAnsweringRepository _sessionsWithFreeAnsweringRepository;
    private readonly ISessionsWithSequentialAnsweringRepository _sessionsWithSequentialAnsweringRepository;

    public StartVokiTakingCommandHandler(
        IGeneralVokisRepository generalVokisRepository,
        IUserCtxProvider userCtxProvider,
        IDateTimeProvider dateTimeProvider,
        IBaseTakingSessionsRepository baseTakingSessionsRepository,
        ISessionsWithFreeAnsweringRepository sessionsWithFreeAnsweringRepository,
        ISessionsWithSequentialAnsweringRepository sessionsWithSequentialAnsweringRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userCtxProvider = userCtxProvider;
        _dateTimeProvider = dateTimeProvider;
        _baseTakingSessionsRepository = baseTakingSessionsRepository;
        _sessionsWithFreeAnsweringRepository = sessionsWithFreeAnsweringRepository;
        _sessionsWithSequentialAnsweringRepository = sessionsWithSequentialAnsweringRepository;
    }


    public async Task<ErrOr<IStartVokiTakingCommandResult>> Handle(StartVokiTakingCommand command, CancellationToken ct) {
        IUserCtx currentTaker = _userCtxProvider.Current;
        if (currentTaker.IsAuthenticated(out var aUserCtx)) {
            var unfinishedSession =
                await _baseTakingSessionsRepository.GetUserUnfinishedSessionBriefData(command.VokiId, aUserCtx, ct);
            if (unfinishedSession is not null) {
                if (command.TerminateExistingUnfinishedSession) {
                    await this.TerminateExistingUnfinishedSession(
                        unfinishedSession.Value.Id, unfinishedSession.Value.IsWithForceSequentialAnswering, ct
                    );
                }
                else {
                    var startedSession = await _baseTakingSessionsRepository.GetForVokiAndUser(command.VokiId, aUserCtx, ct);
                    if (startedSession is null) {
                        return ErrFactory.NotFound.Common(
                            "There is an unfinished session, but it couldn't be loaded. Please try again later or terminate it and start a new one"
                        );
                    }

                    return StartVokiTakingCommandUnfinishedSessionExistsResult.Create(startedSession);
                }
            }
        }

        GeneralVoki? voki = await _generalVokisRepository.GetWithQuestions(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested Voki was not found", $"Voki with id: {command.VokiId} does not exist"
            );
        }

        ErrOr<BaseVokiTakingSession> startRes = voki.StartTaking(currentTaker, _dateTimeProvider.UtcNow);
        if (startRes.IsErr(out var err)) {
            return err;
        }

        var takingSession = startRes.AsSuccess();
        await _baseTakingSessionsRepository.Add(takingSession, ct);
        return SuccessStartVokiTakingCommandResult.Create(voki, takingSession);
    }

    private async Task TerminateExistingUnfinishedSession(
        VokiTakingSessionId id, bool isWithForceSequentialAnswering, CancellationToken ct
    ) {
        if (isWithForceSequentialAnswering) {
            var session = await _sessionsWithSequentialAnsweringRepository.GetByIdForUpdate(id, ct);
            if (session is not null) {
                await _sessionsWithSequentialAnsweringRepository.Delete(session, ct);
            }
        }
        else {
            var session = await _sessionsWithFreeAnsweringRepository.GetByIdForUpdate(id, ct);
            if (session is not null) {
                await _sessionsWithFreeAnsweringRepository.Delete(session, ct);
            }
        }
    }
}

public interface IStartVokiTakingCommandResult;

public record StartVokiTakingCommandUnfinishedSessionExistsResult(
    SavedUnfinishedVokiTakingSessionDto SessionData
) : IStartVokiTakingCommandResult
{
    public static StartVokiTakingCommandUnfinishedSessionExistsResult Create(BaseVokiTakingSession takingSession) => new(
        SavedUnfinishedVokiTakingSessionDto.Create(takingSession)
    );
}

public record SuccessStartVokiTakingCommandResult(
    CurrentVokiTakingSessionDto SessionData
) : IStartVokiTakingCommandResult
{
    public static SuccessStartVokiTakingCommandResult Create(GeneralVoki voki, BaseVokiTakingSession takingSession) => new(
        CurrentVokiTakingSessionDto.Create(
            vokiName: voki.Name,
            vokiQuestionsById: voki.Questions.ToDictionary(q => q.Id, q => q),
            takingSession,
            questionsToShow: takingSession.QuestionsToShowOnStart()
        )
    );
}