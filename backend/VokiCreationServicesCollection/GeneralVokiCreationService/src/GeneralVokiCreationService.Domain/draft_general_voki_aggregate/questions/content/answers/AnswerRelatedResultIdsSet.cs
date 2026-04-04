using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;

public sealed class AnswerRelatedResultIdsSet : ValueObject
{
    public const int MaxRelatedResultsForAnswerCount = 30;
    private ImmutableHashSet<GeneralVokiResultId> ResultIds { get; }

    public bool IsEmpty => ResultIds.Count == 0;

    private AnswerRelatedResultIdsSet(ImmutableHashSet<GeneralVokiResultId> resultIds) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(resultIds));
        ResultIds = resultIds;
    }

    public static ErrOr<AnswerRelatedResultIdsSet> Create(
        ImmutableHashSet<GeneralVokiResultId>? resultIds
    ) {
        if (resultIds is null) {
            return new AnswerRelatedResultIdsSet([]);
        }

        if (CheckForErr(resultIds).IsErr(out var err)) {
            return err;
        }

        return new AnswerRelatedResultIdsSet(resultIds);
    }

    private static ErrOrNothing CheckForErr(ImmutableHashSet<GeneralVokiResultId> resultIds) =>
        resultIds.Count > MaxRelatedResultsForAnswerCount
            ? ErrFactory.LimitExceeded(
                "Too many related results for answer selected",
                $"Maximum allowed count: {MaxRelatedResultsForAnswerCount}. Selected : {resultIds.Count}"
            )
            : ErrOrNothing.Nothing;

    public int Count => ResultIds.Count;
    public override IEnumerable<object> GetEqualityComponents() => [..ResultIds];

    public AnswerRelatedResultIdsSet Remove(GeneralVokiResultId id) => new(ResultIds.Remove(id));

    public IEnumerable<T> Select<T>(Func<GeneralVokiResultId, T> func) => ResultIds.Select(func);
    public GeneralVokiResultId[] ToArray() => ResultIds.ToArray();
}