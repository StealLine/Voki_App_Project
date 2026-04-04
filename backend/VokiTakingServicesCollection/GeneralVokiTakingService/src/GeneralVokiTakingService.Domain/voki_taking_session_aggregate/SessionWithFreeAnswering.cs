using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.free_answering;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public sealed class SessionWithFreeAnswering : BaseVokiTakingSession
{
    private SessionWithFreeAnswering() { }

    public override bool IsWithForceSequentialAnswering => false;
    private SessionWithFreeAnsweringSavedState _savedState;


    private SessionWithFreeAnswering(
        VokiTakingSessionId id, VokiId vokiId, AppUserId? vokiTaker, DateTime startTime,
        ImmutableArray<TakingSessionExpectedQuestion> questions
    ) : base(id, vokiId, vokiTaker, startTime, questions) {
        _savedState = SessionWithFreeAnsweringSavedState.CreateEmpty(questions, startTime);
    }

    public static SessionWithFreeAnswering Create(
        VokiId vokiId, IUserCtx vokiTakerCtx, DateTime startTime,
        IEnumerable<VokiQuestion> vokiQuestions, bool shuffleQuestions
    ) {
        VokiQuestion[] ordered = (
            shuffleQuestions
                ? vokiQuestions.OrderBy(_ => Random.Shared.Next())
                : vokiQuestions.OrderBy(q => q.OrderInVoki.Value)
        ).ToArray();

        List<TakingSessionExpectedQuestion> sessionQuestion = new(ordered.Length);
        for (ushort i = 0; i < ordered.Length; i++) {
            VokiQuestion question = ordered[i];
            sessionQuestion.Add(new TakingSessionExpectedQuestion(
                question.Id,
                OrderInVokiTaking: QuestionOrderInVokiTakingSession.Create(i + 1).AsSuccess(),
                question.AnswersCountLimit.MinAnswers,
                question.AnswersCountLimit.MaxAnswers,
                question.GetAnswerOrderForVokiTakingSession()
            ));
        }

        AppUserId? vokiTakerId = vokiTakerCtx.Match<AppUserId?>(a => a.UserId, _ => null);
        return new SessionWithFreeAnswering(
            VokiTakingSessionId.CreateNew(), vokiId, vokiTakerId, startTime,
            sessionQuestion.ToImmutableArray()
        );
    }

    //all questions are available for user from the start
    public override ImmutableArray<TakingSessionExpectedQuestion> QuestionsToShowOnStart() =>
        Questions.ToImmutableArray();

    public override ErrOr<VokiTakingStateToContinueFromSaved> GetSavedStateToContinueTaking(AuthenticatedUserCtx userCtx) {
        if (this.ValidateProvidedTaker(userCtx).IsErr(out var err)) {
            return err;
        }
        ImmutableHashSet<GeneralVokiQuestionId> answeredQuestionIds = _savedState.QuestionsWithAnswers
            .Where(qWithA => qWithA.Value.Count > 0)
            .Select(qWithA => qWithA.Key)
            .ToImmutableHashSet();
        TakingSessionExpectedQuestion? firstUnansweredQuestion = Questions
            .Where(q => !answeredQuestionIds.Contains(q.QuestionId))
            .MinBy(q => q.OrderInVokiTaking.Value);

        if (firstUnansweredQuestion is null) {
            firstUnansweredQuestion = Questions.MaxBy(q => q.OrderInVokiTaking.Value)!;
        }

        var questionsToShow = Questions
            .Select(q => (
                    q,
                    _savedState.QuestionsWithAnswers.TryGetValue(q.QuestionId, out var saved)
                        ? saved
                        : ImmutableHashSet<GeneralVokiAnswerId>.Empty
                )
            )
            .ToImmutableArray();

        return new VokiTakingStateToContinueFromSaved(
            firstUnansweredQuestion.QuestionId,
            questionsToShow
        );
    }

    public override ushort QuestionsWithSavedAnswersCount() =>
        (ushort)_savedState.QuestionsWithAnswers
            .Count(a => a.Value.Count > 0);

    public ErrOr<VokiTakingSessionFinishedDto> FinishAndReceiveResult(
        DateTime currentTime,
        ClientServerTimePairDto sessionStartTime,
        DateTime clientFinishedTime,
        IUserCtx userCtx,
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> chosenAnswers,
        Func<Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>, ErrOr<GeneralVokiResultId>>
            getResultByToAnswers
    ) {
        if (
            ValidateStartAndFinishTime(
                currentTime: currentTime,
                sessionStartTime: sessionStartTime,
                clientFinishTime: clientFinishedTime
            ).IsErr(out var err)
        ) {
            return err;
        }


        if (ValidateProvidedTakerAndSetIfOk(userCtx).IsErr(out err)) {
            return err;
        }

        if (ValidateChosenAnswers(chosenAnswers).IsErr(out err)) {
            return err;
        }

        ErrOr<GeneralVokiResultId> resOrErr = getResultByToAnswers(chosenAnswers);
        if (resOrErr.IsErr(out err)) {
            return err;
        }

        var questionIdToOrder = Questions.ToImmutableDictionary(
            q => q.QuestionId,
            q => q.OrderInVokiTaking
        );

        ImmutableArray<VokiTakenQuestionDetails> questionDetails = chosenAnswers
            .Select(qToAnswers => new VokiTakenQuestionDetails(
                qToAnswers.Key,
                qToAnswers.Value,
                questionIdToOrder[qToAnswers.Key]
            ))
            .ToImmutableArray();

        return new VokiTakingSessionFinishedDto(
            VokiId,
            VokiTaker,
            sessionStartTime.Client,
            clientFinishedTime,
            WasSessionWithForceSequentialOrder: false,
            ReceivedResultId: resOrErr.AsSuccess(),
            questionDetails
        );
    }


    public ErrOr<SessionWithFreeAnsweringSavedState> SaveCurrentState(
        IUserCtx userCtx,
        VokiId vokiId,
        ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> stateToSave,
        DateTime saveTime
    ) {
        if (this.VokiId != vokiId) {
            return ErrFactory.Conflict("Provided save data does not belong to this Voki");
        }

        if (ValidateProvidedTakerAndSetIfOk(userCtx).IsErr(out var err)) {
            return err.WithMessagePrefix("Could not save answers. ");
        }

        if (saveTime < this._savedState.SaveTime) {
            return ErrFactory.Conflict("Provided save is earlier than the time of the last save");
        }

        if (stateToSave.Count > Questions.Length) {
            return ErrFactory.Conflict(
                "Cannot save current voki taking session state because save provides data for more questions than the Voki has",
                $"Voki questions count: {Questions.Length}. Questions in the provided state to save: {stateToSave.Count}"
            );
        }

        ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> existingQuestions = Questions
            .ToImmutableDictionary(
                q => q.QuestionId,
                q => q.AnswersIdToOrderInQuestion.Keys.ToImmutableHashSet()
            );
        ErrOrNothing errs = ErrOrNothing.Nothing;
        foreach (var (qId, answerIds) in stateToSave) {
            if (!existingQuestions.TryGetValue(qId, out var expectedAnswers)) {
                errs.AddNext(ErrFactory.NotFound.VokiContent($"There are no question with id: {qId}"));
            }
            else if (answerIds.Count > expectedAnswers.Count) {
                errs.AddNext(ErrFactory.NotFound.VokiContent($"To many answers selected for save for question with id: {qId}"));
            }
            else if (answerIds.Any(answerId => !expectedAnswers.Contains(answerId))) {
                errs.AddNext(ErrFactory.NotFound.VokiContent($"There unexpected answer for question with id: {qId}"));
            }
        }

        if (errs.IsErr(out err)) {
            return err;
        }

        _savedState = new SessionWithFreeAnsweringSavedState(stateToSave, saveTime);
        return _savedState;
    }

    private ErrOrNothing ValidateChosenAnswers(
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>? chosenAnswers
    ) {
        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (chosenAnswers is null) {
            errs.AddNext(ErrFactory.NoValue.Common("Chosen answers are missing"));
            return errs;
        }

        ImmutableDictionary<GeneralVokiQuestionId, TakingSessionExpectedQuestion> expectedById = Questions
            .ToImmutableDictionary(q => q.QuestionId, q => q);

        foreach (var providedQuestionId in chosenAnswers.Keys) {
            if (!expectedById.ContainsKey(providedQuestionId)) {
                errs.AddNext(ErrFactory.IncorrectFormat(
                    "You answered a question that is not part of this test"
                ));
            }
        }

        foreach (var kv in expectedById) {
            var question = kv.Value;
            chosenAnswers.TryGetValue(kv.Key, out var providedSet);

            errs.AddNextIfErr(ValidateSingleQuestionAnswers(question, providedSet));
        }

        return errs;
    }
}