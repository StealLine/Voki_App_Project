using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;

namespace GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;

public sealed record AnswerQuestionInSequentialAnsweringVokiTakingCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    GeneralVokiQuestionId QuestionId,
    ClientServerTimePairDto ShownAt,
    DateTime ClientQuestionAnsweredAt,
    QuestionOrderInVokiTakingSession QuestionOrderInVokiTaking,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswers
) : ICommand<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult>;

internal sealed class AnswerQuestionInSequentialAnsweringVokiTakingCommandHandler : ICommandHandler<
    AnswerQuestionInSequentialAnsweringVokiTakingCommand,
    AnswerQuestionInSequentialAnsweringVokiTakingCommandResult
>
{
    private readonly ISessionsWithSequentialAnsweringRepository _sessionsWithSequentialAnsweringRepository;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AnswerQuestionInSequentialAnsweringVokiTakingCommandHandler(
        ISessionsWithSequentialAnsweringRepository sessionsWithSequentialAnsweringRepository,
        IGeneralVokisRepository generalVokisRepository, IUserCtxProvider userCtxProvider, IDateTimeProvider dateTimeProvider
    ) {
        _sessionsWithSequentialAnsweringRepository = sessionsWithSequentialAnsweringRepository;
        _generalVokisRepository = generalVokisRepository;
        _userCtxProvider = userCtxProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult>> Handle(
        AnswerQuestionInSequentialAnsweringVokiTakingCommand command, CancellationToken ct
    ) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithQuestions(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Cannot answer the question because requested Voki does not exist");
        }

        SessionWithSequentialAnswering? session =
            await _sessionsWithSequentialAnsweringRepository.GetByIdForUpdate(command.SessionId, ct);
        if (session is null) {
            return ErrFactory.NotFound.Common(
                "Cannot answer the question because Voki taking session with sequential answering was not found "
            );
        }

        ErrOr<TakingSessionExpectedQuestion> answeringResult = session.AnswerQuestionAndGetNext(
            command.VokiId,
            shownAt: command.ShownAt,
            currentTime: _dateTimeProvider.UtcNow,
            clientQuestionAnsweredAt: command.ClientQuestionAnsweredAt,
            questionId: command.QuestionId,
            questionOrderInVokiTaking: command.QuestionOrderInVokiTaking,
            chosenAnswers: command.ChosenAnswers
        );
        if (answeringResult.IsErr(out var err)) {
            return err;
        }

        TakingSessionExpectedQuestion nextQuestion = answeringResult.AsSuccess();
        VokiQuestion? vokiQuestion = voki.Questions.FirstOrDefault(q => q.Id == nextQuestion.QuestionId);
        if (vokiQuestion is null) {
            return ErrFactory.NotFound.VokiContent("Expected next question was not found in Voki");
        }

        await _sessionsWithSequentialAnsweringRepository.Update(session, ct);

        return AnswerQuestionInSequentialAnsweringVokiTakingCommandResult.Create(
            vokiQuestion, nextQuestion, _dateTimeProvider.UtcNow
        );
    }
}

public sealed record AnswerQuestionInSequentialAnsweringVokiTakingCommandResult(
    VokiTakingQuestionData Question,
    DateTime CurrentTime
)
{
    public static AnswerQuestionInSequentialAnsweringVokiTakingCommandResult Create(
        VokiQuestion vokiQuestion,
        TakingSessionExpectedQuestion questionDataInSession, DateTime currentTime
    ) => new(
        VokiTakingQuestionData.Create(
            vokiQuestion,
            questionDataInSession.OrderInVokiTaking,
            questionDataInSession.AnswersIdToOrderInQuestion
        ),
        currentTime
    );
}