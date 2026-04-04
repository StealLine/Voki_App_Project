using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.common.vokis;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record ContinueVokiTakingCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId
) : ICommand<ContinueVokiTakingCommandResult>;

internal sealed class ContinueVokiTakingCommandHandler
    : ICommandHandler<ContinueVokiTakingCommand, ContinueVokiTakingCommandResult>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IBaseTakingSessionsRepository _baseTakingSessionsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ContinueVokiTakingCommandHandler(
        IUserCtxProvider userCtxProvider,
        IGeneralVokisRepository generalVokisRepository,
        IBaseTakingSessionsRepository baseTakingSessionsRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _userCtxProvider = userCtxProvider;
        _generalVokisRepository = generalVokisRepository;
        _baseTakingSessionsRepository = baseTakingSessionsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<ContinueVokiTakingCommandResult>> Handle(ContinueVokiTakingCommand command, CancellationToken ct) {
        var vokiTakerCtx = _userCtxProvider.Current;
        if (!vokiTakerCtx.IsAuthenticated(out var aUserCtx)) {
            return ErrFactory.AuthRequired("You must be authenticated to continue voki taking");
        }

        BaseVokiTakingSession? session = await _baseTakingSessionsRepository.GetById(command.SessionId, ct);
        if (session is null) {
            return ErrFactory.NotFound.Voki(
                "Unfinished session not found",
                "No unfinished voki taking session found for this user and voki"
            );
        }

        var voki = await _generalVokisRepository.GetWithQuestions(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki not found", $"Voki with id: {command.VokiId} does not exist");
        }

        return ContinueVokiTakingCommandResult.Create(
            aUserCtx,
            voki.Name,
            vokiQuestionsById: voki.Questions.ToDictionary(q => q.Id, q => q),
            session,
            _dateTimeProvider.UtcNow
        );
    }
}

public record ContinueVokiTakingCommandResult(
    GeneralVokiQuestionId CurrentQuestionId,
    CurrentVokiTakingSessionDto SessionData,
    ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> SavedChosenAnswers,
    DateTime CurrentDateTime
)
{
    public static ErrOr<ContinueVokiTakingCommandResult> Create(
        AuthenticatedUserCtx aUserCtx,
        VokiName vokiName,
        IDictionary<GeneralVokiQuestionId, VokiQuestion> vokiQuestionsById,
        BaseVokiTakingSession session,
        DateTime currentDateTime
    ) {
        var savedStateRes = session.GetSavedStateToContinueTaking(aUserCtx);
        if (savedStateRes.IsErr(out var err)) {
            return err;
        }

        var savedState = savedStateRes.AsSuccess();
        var questions = savedState.QuestionsToShow;

        CurrentVokiTakingSessionDto sessionDto = CurrentVokiTakingSessionDto.Create(
            vokiName, vokiQuestionsById,
            session,
            questionsToShow: questions.Select(q => q.Item1)
        );
        return new ContinueVokiTakingCommandResult(
            savedState.CurrentQuestionId,
            sessionDto,
            questions.ToImmutableDictionary(
                q => q.Item1.QuestionId,
                q => q.savedAnswerIds
            ),
            currentDateTime
        );
    }
}