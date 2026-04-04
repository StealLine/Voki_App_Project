using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.free_answering;
using SharedKernel;

namespace GeneralVokiTakingService.Application.general_vokis.commands.free_answering_voki_taking;

public record SaveCurrentFreeVokiTakingSessionStateCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> ChosenAnswers
) : ICommand<SessionWithFreeAnsweringSavedState>;

internal sealed class SaveCurrentFreeVokiTakingSessionStateCommandHandler
    : ICommandHandler<SaveCurrentFreeVokiTakingSessionStateCommand, SessionWithFreeAnsweringSavedState>
{
    private readonly ISessionsWithFreeAnsweringRepository _sessionsWithFreeAnsweringRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SaveCurrentFreeVokiTakingSessionStateCommandHandler(
        ISessionsWithFreeAnsweringRepository sessionsWithFreeAnsweringRepository,
        IUserCtxProvider userCtxProvider,
        IDateTimeProvider dateTimeProvider
    ) {
        _sessionsWithFreeAnsweringRepository = sessionsWithFreeAnsweringRepository;
        _userCtxProvider = userCtxProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<SessionWithFreeAnsweringSavedState>> Handle(
        SaveCurrentFreeVokiTakingSessionStateCommand command, CancellationToken ct
    ) {
        SessionWithFreeAnswering? session = await _sessionsWithFreeAnsweringRepository.GetByIdForUpdate(command.SessionId, ct);
        if (session is null) {
            return ErrFactory.NotFound.Common("Session not found");
        }


        ErrOr<SessionWithFreeAnsweringSavedState> saveRes = session.SaveCurrentState(
            _userCtxProvider.Current,
            command.VokiId,
            command.ChosenAnswers,
            _dateTimeProvider.UtcNow
        );
        if (saveRes.IsErr(out var err)) {
            return err;
        }

        await _sessionsWithFreeAnsweringRepository.Update(session, ct);
        return saveRes.AsSuccess();
    }
}