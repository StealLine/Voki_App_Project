using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands.free_answering_voki_taking;

public sealed record FinishVokiTakingWithFreeAnsweringCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> ChosenAnswers,
    ClientServerTimePairDto SessionStartTime,
    DateTime ClientFinishTime
) : ICommand<GeneralVokiResultId>;

internal sealed class FinishVokiTakingWithFreeAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithFreeAnsweringCommand, GeneralVokiResultId>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISessionsWithFreeAnsweringRepository _sessionsWithFreeAnsweringRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IGeneralVokiTakenRecordsRepository _generalVokiTakenRecordsRepository;

    public FinishVokiTakingWithFreeAnsweringCommandHandler(
        IGeneralVokisRepository generalVokisRepository,
        IDateTimeProvider dateTimeProvider,
        ISessionsWithFreeAnsweringRepository sessionsWithFreeAnsweringRepository,
        IGeneralVokiTakenRecordsRepository generalVokiTakenRecordsRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _generalVokisRepository = generalVokisRepository;
        _dateTimeProvider = dateTimeProvider;
        _sessionsWithFreeAnsweringRepository = sessionsWithFreeAnsweringRepository;
        _generalVokiTakenRecordsRepository = generalVokiTakenRecordsRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<GeneralVokiResultId>> Handle(FinishVokiTakingWithFreeAnsweringCommand command,
        CancellationToken ct) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithQuestionsAndResults(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Cannot finish voki taking because requested Voki does not exist");
        }

        SessionWithFreeAnswering? session = await _sessionsWithFreeAnsweringRepository.GetByIdForUpdate(command.SessionId, ct);
        if (session is null) {
            return ErrFactory.NotFound.Common(
                "Could not finish voki taking because taking session was not started",
                "Most likely if you see this message you have already finished your taking session"
            );
        }

        ErrOr<VokiTakingSessionFinishedDto> sessionFinishRes = session.FinishAndReceiveResult(
            _dateTimeProvider.UtcNow,
            command.SessionStartTime,
            command.ClientFinishTime,
            _userCtxProvider.Current,
            command.ChosenAnswers,
            (answ) => voki.GetResultIdByChosenAnswers(answ)
        );
        if (sessionFinishRes.IsErr(out var err)) {
            return err;
        }

        VokiTakingSessionFinishedDto sessionFinishedResult = sessionFinishRes.AsSuccess();
        GeneralVokiTakenRecord vokiTakenRecord = GeneralVokiTakenRecord.CreateNew(sessionFinishedResult);
        await _generalVokiTakenRecordsRepository.Add(vokiTakenRecord, ct);
        await _sessionsWithFreeAnsweringRepository.Delete(session, ct);

        return sessionFinishedResult.ReceivedResultId;
    }
}