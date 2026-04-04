using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate.events;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.exceptions;
using SharedKernel.user_ctx;
using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.concrete_keys;
using VokiTakingServicesLib.Domain.base_voki_aggregate;
using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public sealed class GeneralVoki : BaseVoki
{
    private GeneralVoki() { }
    public IReadOnlyCollection<VokiQuestion> Questions { get; }
    private bool ForceSequentialAnswering { get; }
    public bool ShuffleQuestions { get; }
    private IReadOnlyCollection<VokiResult> _results { get; }
    private ImmutableHashSet<VokiTakenRecordId> VokiTakenRecordIds { get; set; }
    public GeneralVokiInteractionSettings InteractionSettings { get; private set; }
    protected override BaseVokiInteractionSettings BaseInteractionSettings => InteractionSettings;

    public GeneralVoki(
        VokiId id, VokiName name, ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results,
        bool forceSequentialAnswering, bool shuffleQuestions, GeneralVokiInteractionSettings interactionSettings,
        VokiManagersIdsSet managers
    ) : base(name, managers) {
        Id = id;
        Questions = questions;
        _results = results;
        ForceSequentialAnswering = forceSequentialAnswering;
        ShuffleQuestions = shuffleQuestions;
        InteractionSettings = interactionSettings;
        ShuffleQuestions = shuffleQuestions;
        VokiTakenRecordIds = [];
    }


    public static GeneralVoki CreateNew(
        VokiId id, VokiCoverKey coverKey, VokiName name,
        ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results,
        bool forceSequentialAnswering, bool shuffleQuestions, GeneralVokiInteractionSettings interactionSettings,
        VokiManagersIdsSet managers
    ) {
        GeneralVoki voki = new(
            id, name, questions, results,
            forceSequentialAnswering: forceSequentialAnswering,
            shuffleQuestions: shuffleQuestions,
            interactionSettings, managers
        );
        List<BaseStorageKey> vokiContentKeys = GatherVokiContentKeys(coverKey, questions, results);
        voki.AddDomainEvent(new PublishedVokiCreatedEvent(voki.Id, vokiContentKeys.ToArray()));
        return voki;
    }

    public uint ResultsCount => (uint)_results.Count;

    private static List<BaseStorageKey> GatherVokiContentKeys(
        VokiCoverKey coverKey, ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results
    ) {
        List<BaseStorageKey> keys = results
            .Select(r => r.Image)
            .OfType<BaseStorageKey>() //skipping nulls and casting to non-nullable
            .ToList();

        foreach (var question in questions) {
            keys.AddRange(question.ImageSet.Keys.Select(k => k as BaseStorageKey));
            keys.AddRange(question.Content.GatherContentKeys());
        }

        keys.Add(coverKey);

        return keys;
    }

    public ErrOr<BaseVokiTakingSession> StartTaking(IUserCtx userCtx, DateTime currentTime) {
        if (CheckUserAccessToTake(userCtx).IsErr(out var err)) {
            return err;
        }

        if (ForceSequentialAnswering) {
            return SessionWithSequentialAnswering.Create(Id, userCtx, currentTime, Questions, ShuffleQuestions);
        }
        else {
            return SessionWithFreeAnswering.Create(Id, userCtx, currentTime, Questions, ShuffleQuestions);
        }
    }

    public void AddNewVokiTakenRecord(VokiTakenRecordId newRecordId, AppUserId? vokiTakerId) {
        if (VokiTakenRecordIds.Contains(newRecordId)) {
            return;
        }

        VokiTakenRecordIds = VokiTakenRecordIds.Add(newRecordId);
        AddDomainEvent(new VokiGotNewVokiTakenRecordEvent(Id, vokiTakerId, (uint)VokiTakenRecordIds.Count));
    }

    public ErrOr<GeneralVokiResultId> GetResultIdByChosenAnswers(
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> chosenAnswers
    ) {
        var questionsById = Questions.ToDictionary(q => q.Id);
        foreach (var providedId in chosenAnswers.Keys) {
            if (!questionsById.ContainsKey(providedId)) {
                return ErrFactory.IncorrectFormat("You answered a question that is not part of this test");
            }
        }

        foreach (var q in Questions) {
            if (
                !chosenAnswers.TryGetValue(q.Id, out var provided)
                || provided is null
                || provided.Count == 0
            ) {
                return ErrFactory.NoValue.Common(
                    $"You did not answer the question: \"{q.Preview()}\". {q.ChooseExpectedNumberOfAnswersText()}"
                );
            }

            var count = provided.Count;
            if (count < q.AnswersCountLimit.MinAnswers || count > q.AnswersCountLimit.MaxAnswers) {
                return ErrFactory.ValueOutOfRange(
                    $"Incorrect answers count for \"{q.Preview()}\". {q.ChooseExpectedNumberOfAnswersText()}"
                );
            }

            if (!provided.IsSubsetOf(q.Content.AnswerIds)) {
                return ErrFactory.IncorrectFormat(
                    $"Your selection includes an option that is not available for \"{q.Preview()}\". Please select only from the shown options"
                );
            }
        }

        Dictionary<GeneralVokiResultId, int> resultsScore = new();
        foreach (var q in Questions) {
            ImmutableHashSet<GeneralVokiAnswerId> answersForQuestion = chosenAnswers[q.Id];

            ImmutableDictionary<GeneralVokiAnswerId, BaseQuestionAnswer> answersById = q.Content.AnswerByIds;
            foreach (var ansId in answersForQuestion) {
                foreach (var resultId in answersById[ansId].RelatedResultIds) {
                    resultsScore[resultId] = resultsScore.GetValueOrDefault(resultId, 0) + 1;
                }
            }
        }

        if (resultsScore.Count == 0) {
            return ErrFactory.Conflict("Cannot determine result based on chosen answers");
        }

        var bestResultId = resultsScore.MaxBy(kvp => kvp.Value).Key;
        return _results.First(r => r.Id == bestResultId).Id;
    }


    public ErrOr<VokiResult> GetResultToViewByAnyOne(GeneralVokiResultId resultId) {
        if (InteractionSettings.ResultsVisibility != GeneralVokiResultsVisibility.Anyone) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not show result for anyone, when visibility is set to {GeneralVokiResultsVisibility.Anyone}"
            ));
        }

        var result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return ErrFactory.NotFound.Common("Voki doesn't have requested result");
        }

        return result;
    }

    public ErrOr<VokiResult> GetResultToViewByUserAfterTaking(
        GeneralVokiResultId resultId, ISet<GeneralVokiResultId> userReceivedResultIds
    ) {
        if (InteractionSettings.ResultsVisibility != GeneralVokiResultsVisibility.AfterTaking) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not show result after taking, when visibility is set to {InteractionSettings.ResultsVisibility}"
            ));
        }


        if (!UserHasTakenThisVoki(userReceivedResultIds)) {
            return ErrFactory.NoAccess("To see this result you need to take this voki as a logged in user");
        }

        var result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return ErrFactory.NotFound.Common("Voki doesn't have requested result");
        }

        return result;
    }


    public ErrOr<VokiResult> GetOnlyReceivedResultToViewByUser(
        GeneralVokiResultId resultId, ISet<GeneralVokiResultId> userReceivedResultIds
    ) {
        if (InteractionSettings.ResultsVisibility != GeneralVokiResultsVisibility.OnlyReceived) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not show result only if received, when visibility is set to {InteractionSettings.ResultsVisibility}"
            ));
        }

        if (!userReceivedResultIds.Contains(resultId)) {
            return ErrFactory.NoAccess("To see this result you need to receive it as a logged in user");
        }

        var result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return ErrFactory.NotFound.Common("Voki doesn't have requested result");
        }

        return result;
    }

    public IEnumerable<VokiResult> ResultsReceivedByUser(ISet<GeneralVokiResultId> userReceivedResultIds) =>
        _results.Where(r => userReceivedResultIds.Contains(r.Id));


    public ErrOr<IEnumerable<VokiResultWithDistributionPercent>> AllResultsWithDistributionForAuthenticatedUser(
        ISet<GeneralVokiResultId> userReceivedResultIds,
        IReadOnlyDictionary<GeneralVokiResultId, uint> resultIdsToCount
    ) => InteractionSettings.ResultsVisibility.Match(
        anyone: () => ErrOr<IEnumerable<VokiResultWithDistributionPercent>>.Success(
            AttachDistributionToResults(_results, resultIdsToCount)
        ),
        afterTaking: () => UserHasTakenThisVoki(userReceivedResultIds)
            ? ErrOr<IEnumerable<VokiResultWithDistributionPercent>>.Success(
                AttachDistributionToResults(_results, resultIdsToCount)
            )
            : ErrFactory.NoAccess(
                "To see all voki results you need to take this voki at least one time as a logged in user"),
        onlyReceived: () => UserHasReceivedAllResults(userReceivedResultIds)
            ? ErrOr<IEnumerable<VokiResultWithDistributionPercent>>.Success(
                AttachDistributionToResults(_results, resultIdsToCount)
            )
            : ErrFactory.NoAccess("Author set so that to see all voki results you need to firstly receive them all")
    );

    public ErrOr<IEnumerable<VokiResultWithDistributionPercent>> AllResultsWithDistributionForNonAuthenticatedUser(
        IReadOnlyDictionary<GeneralVokiResultId, uint> resultIdsToCount
    ) => InteractionSettings.ResultsVisibility.Match(
        anyone: () => ErrOr<IEnumerable<VokiResultWithDistributionPercent>>.Success(
            AttachDistributionToResults(_results, resultIdsToCount)
        ),
        afterTaking: () => ErrFactory.NoAccess(
            "To see all voki results you need to take this voki at least one time as a logged in user"
        ),
        onlyReceived: () => ErrFactory.NoAccess(
            "Author set so that to see all voki results you need to firstly receive them all"
        )
    );

    private IEnumerable<VokiResultWithDistributionPercent> AttachDistributionToResults(
        IEnumerable<VokiResult> results, IReadOnlyDictionary<GeneralVokiResultId, uint> resultIdsToCount
    ) {
        if (!InteractionSettings.ShowResultsDistribution || resultIdsToCount.Count == 0) {
            return results
                .Select(r => new VokiResultWithDistributionPercent(r, 0));
        }

        double total = resultIdsToCount.Values.Sum(x => x);
        return results
            .Select(r => {
                resultIdsToCount.TryGetValue(r.Id, out var count);
                double percent = total == 0 ? 0 : count * 100d / total;
                return new VokiResultWithDistributionPercent(r, percent);
            });
    }


    public bool UserHasTakenThisVoki(ISet<GeneralVokiResultId> userReceivedResultIds) =>
        userReceivedResultIds.Overlaps(_results.Select(r => r.Id));

    private bool UserHasReceivedAllResults(ISet<GeneralVokiResultId> userReceivedResultIds) =>
        userReceivedResultIds.IsSupersetOf(_results.Select(r => r.Id));
}