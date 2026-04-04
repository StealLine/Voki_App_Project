using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public sealed class SessionWithSequentialAnswering : BaseVokiTakingSession
{
    private SessionWithSequentialAnswering() { }
    public override bool IsWithForceSequentialAnswering => true;

    private ImmutableArray<SequentialTakingAnsweredQuestion> _answered;

    private SessionWithSequentialAnswering(
        VokiTakingSessionId id, VokiId vokiId, AppUserId? vokiTaker, DateTime startTime,
        ImmutableArray<TakingSessionExpectedQuestion> questions
    ) : base(id, vokiId, vokiTaker, startTime, questions) {
        _answered = [];
    }

    public static SessionWithSequentialAnswering Create(
        VokiId vokiId, IUserCtx vokiTakerCtx, DateTime startTime,
        IEnumerable<VokiQuestion> vokiQuestions, bool shuffleQuestions
    ) {
        VokiQuestion[] ordered = (
            shuffleQuestions
                ? vokiQuestions.OrderBy(_ => Guid.NewGuid())
                : vokiQuestions.OrderBy(q => q.OrderInVoki)
        ).ToArray();

        List<TakingSessionExpectedQuestion> sessionQuestion = new(ordered.Length);
        for (ushort i = 0; i < ordered.Length; i++) {
            var question = ordered[i];
            sessionQuestion.Add(new TakingSessionExpectedQuestion(
                question.Id,
                OrderInVokiTaking: QuestionOrderInVokiTakingSession.Create(i + 1).AsSuccess(),
                question.AnswersCountLimit.MinAnswers,
                question.AnswersCountLimit.MaxAnswers,
                question.GetAnswerOrderForVokiTakingSession()
            ));
        }

        AppUserId? vokiTakerId = vokiTakerCtx.Match<AppUserId?>(a => a.UserId, _ => null);
        return new SessionWithSequentialAnswering(
            VokiTakingSessionId.CreateNew(), vokiId, vokiTakerId, startTime,
            questions: sessionQuestion.ToImmutableArray()
        );
    }

    //questions must be shown one by one 
    public override ImmutableArray<TakingSessionExpectedQuestion> QuestionsToShowOnStart() {
        var firstQuestionInTaking = Questions.MinBy(q => q.OrderInVokiTaking.Value)!;
        return [firstQuestionInTaking];
    }


    public override ErrOr<VokiTakingStateToContinueFromSaved> GetSavedStateToContinueTaking(AuthenticatedUserCtx userCtx) {
        if (this.ValidateProvidedTaker(userCtx).IsErr(out var err)) {
            return err;
        }

        ImmutableHashSet<GeneralVokiQuestionId> answeredQuestionIds = _answered
            .Select(q => q.QuestionId)
            .ToImmutableHashSet();

        TakingSessionExpectedQuestion? firstUnansweredQuestion = Questions
            .Where(q => !answeredQuestionIds.Contains(q.QuestionId))
            .MinBy(q => q.OrderInVokiTaking.Value);

        if (firstUnansweredQuestion is not null) {
            return new VokiTakingStateToContinueFromSaved(
                firstUnansweredQuestion.QuestionId,
                QuestionsToShow: [(firstUnansweredQuestion, ImmutableHashSet<GeneralVokiAnswerId>.Empty)]
            );
        }

        //all questions are answered. Last question will be shown with its saved answers
        var lastQuestion = Questions.MaxBy(q => q.OrderInVokiTaking.Value)!;
        var lastQuestionSavedAnswers = _answered
            .FirstOrDefault(a => a.QuestionId == lastQuestion.QuestionId)
            ?.ChosenAnswerIds ?? ImmutableHashSet<GeneralVokiAnswerId>.Empty;
        return new VokiTakingStateToContinueFromSaved(
            lastQuestion.QuestionId,
            QuestionsToShow: [(lastQuestion, lastQuestionSavedAnswers)]
        );
    }

    public override ushort QuestionsWithSavedAnswersCount() => (ushort)_answered.Length;


    public ErrOr<VokiTakingSessionFinishedDto> FinishAndReceiveResult(
        DateTime currentTime,
        ClientServerTimePairDto sessionStartTime,
        DateTime clientSessionFinishedTime,
        IUserCtx userCtx,
        GeneralVokiQuestionId lastQuestionId,
        QuestionOrderInVokiTakingSession lastQuestionOrderInVokiTaking,
        ClientServerTimePairDto lastQuestionShownAt,
        DateTime clientLastAnsweredAt,
        ImmutableHashSet<GeneralVokiAnswerId> lastChosenAnswers,
        Func<Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>, ErrOr<GeneralVokiResultId>>
            getResultAccordingToAnswers
    ) {
        if (ValidateStartAndFinishTime(
                currentTime: currentTime,
                sessionStartTime: sessionStartTime,
                clientFinishTime: clientSessionFinishedTime
            ).IsErr(out var err)) {
            return err;
        }

        if (ValidateProvidedTakerAndSetIfOk(userCtx).IsErr(out err)) {
            return err;
        }

        var expected = Questions.FirstOrDefault(q => q.QuestionId == lastQuestionId);
        if (expected is null) {
            return ErrFactory.NotFound.Common(
                "This question is not expected in this Voki taking session",
                "Try reloading the page"
            );
        }

        TakingSessionExpectedQuestion? current = GetFirstUnanswered();
        if (current is null) {
            return ErrFactory.Conflict(
                "There are no questions left to answer",
                "Use the finish-taking action to complete the attempt"
            );
        }

        if (current.QuestionId != lastQuestionId) {
            return ErrFactory.Conflict(
                "You cannot finish the session with this question",
                $"Sequential mode is enabled. Please answer the current question (order {current.OrderInVokiTaking}) first"
            );
        }

        if (!IsLastRemaining(lastQuestionId)) {
            return ErrFactory.Conflict(
                "You cannot finish the session yet",
                "There are unanswered questions left. Answer them first or continue sequentially."
            );
        }

        if (ValidateAnswerForQuestion(expected, lastQuestionOrderInVokiTaking,
                lastQuestionShownAt,
                currentTime, clientLastAnsweredAt,
                lastChosenAnswers
            ).IsErr(out err)) {
            return err;
        }

        SequentialTakingAnsweredQuestion answeredRecord = new SequentialTakingAnsweredQuestion(
            expected.QuestionId,
            lastChosenAnswers,
            lastQuestionShownAt.Client,
            clientLastAnsweredAt
        );
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> answeredWithNew = _answered
            .Add(answeredRecord)
            .ToDictionary(
                a => a.QuestionId,
                a => a.ChosenAnswerIds
            );

        bool notAnsweredLeft = Questions.Any(q => !answeredWithNew.ContainsKey(q.QuestionId));
        if (notAnsweredLeft) {
            return ErrFactory.Conflict(
                "There are unanswered questions left",
                "Please answer remaining questions before finishing."
            );
        }


        var resOrErr = getResultAccordingToAnswers(answeredWithNew);
        if (resOrErr.IsErr(out err)) {
            return err;
        }

        _answered = _answered.Add(answeredRecord);

        var questionIdToOrder = Questions.ToImmutableDictionary(
            q => q.QuestionId,
            q => q.OrderInVokiTaking
        );

        ImmutableArray<VokiTakenQuestionDetails> questionDetails = _answered
            .Select(q => new VokiTakenQuestionDetails(
                q.QuestionId,
                q.ChosenAnswerIds,
                questionIdToOrder[q.QuestionId]
            ))
            .ToImmutableArray();

        return new VokiTakingSessionFinishedDto(
            VokiId,
            VokiTaker,
            sessionStartTime.Client,
            clientSessionFinishedTime,
            WasSessionWithForceSequentialOrder: true,
            ReceivedResultId: resOrErr.AsSuccess(),
            questionDetails
        );
    }

    public ErrOr<TakingSessionExpectedQuestion> AnswerQuestionAndGetNext(
        VokiId vokiId,
        ClientServerTimePairDto shownAt,
        DateTime currentTime,
        DateTime clientQuestionAnsweredAt,
        GeneralVokiQuestionId questionId,
        QuestionOrderInVokiTakingSession questionOrderInVokiTaking,
        ImmutableHashSet<GeneralVokiAnswerId> chosenAnswers
    ) {
        if (this.VokiId != vokiId) {
            return ErrFactory.Conflict("Provided VokiId does not match with the session VokiId");
        }

        TakingSessionExpectedQuestion? expected = Questions.FirstOrDefault(q => q.QuestionId == questionId);
        if (expected is null) {
            return ErrFactory.NotFound.Common(
                "This question is not expected in this Voki taking session",
                "Try reloading the page"
            );
        }

        var alreadyAnswered = _answered.FirstOrDefault(a => a.QuestionId == questionId);
        if (alreadyAnswered is not null) {
            return ErrFactory.Conflict("This question has already been answered", "Try reloading the page");
        }

        return HandleNotYetAnswered(
            expected,
            shownAt,
            currentTime,
            clientQuestionAnsweredAt,
            questionOrderInVokiTaking,
            chosenAnswers
        );
    }


    private ErrOr<TakingSessionExpectedQuestion> HandleNotYetAnswered(
        TakingSessionExpectedQuestion expected,
        ClientServerTimePairDto shownAt,
        DateTime currentTime,
        DateTime clientQuestionAnsweredAt,
        QuestionOrderInVokiTakingSession questionOrderInVokiTaking,
        ImmutableHashSet<GeneralVokiAnswerId> chosenAnswers
    ) {
        TakingSessionExpectedQuestion? current = GetFirstUnanswered();
        if (current is null) {
            return ErrFactory.Conflict(
                "There are no questions left to answer",
                "Use the finish-taking action to complete the attempt"
            );
        }

        if (current.QuestionId != expected.QuestionId) {
            return ErrFactory.Conflict(
                "You cannot answer this question yet",
                $"Sequential mode is enabled. Please answer the current question (order {current.OrderInVokiTaking}) first"
            );
        }

        ErrOrNothing answerValidationRes = ValidateAnswerForQuestion(
            expected, questionOrderInVokiTaking, shownAt,
            currentTime, clientQuestionAnsweredAt, chosenAnswers
        );
        if (answerValidationRes.IsErr(out var err)) {
            return err;
        }

        if (IsLastRemaining(expected.QuestionId)) {
            return ErrFactory.Conflict(
                "Cannot answer and go to next: this is the last question.",
                "Use the finish-taking action to submit and complete the taking session."
            );
        }

        SequentialTakingAnsweredQuestion answeredRecord = new SequentialTakingAnsweredQuestion(
            expected.QuestionId,
            chosenAnswers,
            shownAt.Client,
            clientQuestionAnsweredAt
        );

        _answered = _answered.Add(answeredRecord);
        TakingSessionExpectedQuestion? next = GetFirstUnanswered();
        if (next is null) {
            return ErrFactory.Conflict(
                "There are no questions left to answer",
                "Use the finish-taking action to complete the attempt."
            );
        }

        return next;
    }

    private ErrOrNothing ValidateAnswerForQuestion(
        TakingSessionExpectedQuestion expected,
        QuestionOrderInVokiTakingSession receivedOrderInVokiTaking,
        ClientServerTimePairDto shownAt,
        DateTime currentTime,
        DateTime clientQuestionAnsweredAt,
        ImmutableHashSet<GeneralVokiAnswerId> chosenAnswers
    ) {
        if (expected.OrderInVokiTaking != receivedOrderInVokiTaking) {
            return ErrFactory.Conflict(
                $"Question order mismatch. Expected order {expected.OrderInVokiTaking}, got {receivedOrderInVokiTaking}",
                "Reload the page and continue with the current question"
            );
        }

        if (shownAt.Client < StartTime) {
            return ErrFactory.Conflict(
                "Time sync error: question shown before the voki taking session started",
                "Please reload the page and try again."
            );
        }

        if (clientQuestionAnsweredAt < shownAt.Client) {
            return ErrFactory.Conflict(
                "Time sync error: question submitted earlier than shown",
                "Please reload the page and try again"
            );
        }

        TimeSpan answeredAtSkew = (clientQuestionAnsweredAt - currentTime).Duration();
        if (answeredAtSkew > MaxClockSkew) {
            return ErrFactory.Conflict(
                "Time sync error: the answer is too delayed relative to current time",
                "Please reload the page and try again"
            );
        }

        TimeSpan shownAtSkew = (shownAt.Server - shownAt.Client).Duration();
        if (shownAtSkew > MaxClockSkew) {
            return ErrFactory.Conflict(
                "Time sync error: large difference between server and client time",
                "Please check your device time or reload the page and try again"
            );
        }

        int chosenCount = chosenAnswers.Count;
        if (chosenCount < expected.MinAnswersCount) {
            return ErrFactory.Conflict(
                $"Too few answers selected. Answers chosen: {chosenCount}",
                $"Choose more option(s) to meet the required range from {expected.MinAnswersCount} to {expected.MaxAnswersCount}."
            );
        }

        if (chosenCount > expected.MaxAnswersCount) {
            return ErrFactory.Conflict(
                $"Too many answers selected. Answers chosen: {chosenCount}",
                $"Remove extra option(s) to meet the required range {expected.MinAnswersCount} to {expected.MaxAnswersCount}."
            );
        }

        ImmutableHashSet<GeneralVokiAnswerId> allowedAnswerIds = expected.AnswersIdToOrderInQuestion.Keys.ToImmutableHashSet();
        GeneralVokiAnswerId[] invalidChosen = chosenAnswers
            .Where(a => !allowedAnswerIds.Contains(a))
            .ToArray();

        if (invalidChosen.Length > 0) {
            return ErrFactory.Conflict(
                "Some selected options are not allowed for this question",
                "Clear your selection and choose again from the provided list."
            );
        }

        return ErrOrNothing.Nothing;
    }

    private static readonly TimeSpan MaxClockSkew = TimeSpan.FromMinutes(30);

    private TakingSessionExpectedQuestion? GetFirstUnanswered()
        => Questions
            .Where(q => !_answered.Any(a => a.QuestionId == q.QuestionId))
            .OrderBy(q => q.OrderInVokiTaking)
            .FirstOrDefault();

    private bool IsLastRemaining(GeneralVokiQuestionId questionId) {
        var unansweredCount = Questions.Count(q => !_answered.Any(a => a.QuestionId == q.QuestionId));
        if (unansweredCount == 1) {
            var only = Questions.First(q => !_answered.Any(a => a.QuestionId == q.QuestionId));
            return only.QuestionId == questionId;
        }

        return false;
    }
}