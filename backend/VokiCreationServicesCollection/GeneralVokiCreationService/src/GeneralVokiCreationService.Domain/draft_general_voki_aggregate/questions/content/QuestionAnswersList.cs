using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;

public sealed class QuestionAnswersList<T> : ValueObject where T : BaseQuestionAnswer
{
    private ImmutableArray<T> Items { get; }

    private QuestionAnswersList(IEnumerable<T> items) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(items, out var array));
        Items = array.ToImmutableArray();
    }

    public static ErrOr<QuestionAnswersList<T>> Create(IEnumerable<T> answers) =>
        CheckForErr(answers, out var array).IsErr(out var err)
            ? err
            : new QuestionAnswersList<T>(array);


    private static ErrOrNothing CheckForErr(IEnumerable<T> answers, out T[] answersArray) {
        answersArray = answers as T[] ?? answers.ToArray();

        if (answersArray.Length > VokiQuestion.MaxAnswersCount) {
            return ErrFactory.LimitExceeded(
                $"Answer count limit exceeded. Maximum allowed answers count is {VokiQuestion.MaxAnswersCount}",
                $"Current answers count is {answersArray.Length}"
            );
        }

        if (answersArray.Length == 0) {
            return ErrOrNothing.Nothing;
        }

        var orders = answersArray
            .Select(a => a.Order.Value)
            .ToArray();

        var expectedCount = orders.Length;

        var uniqueOrders = orders
            .Distinct()
            .OrderBy(o => o)
            .ToArray();

        if (uniqueOrders.Length != expectedCount) {
            return ErrFactory.IncorrectFormat(
                "Answer order contains duplicate values",
                $"Orders: {string.Join(", ", orders)}"
            );
        }

        for (var i = 0; i < expectedCount; i++) {
            int expectedOrder = i + 1;
            if (uniqueOrders[i] != expectedOrder) {
                return ErrFactory.IncorrectFormat(
                    "Answer order must be sequential starting from 1",
                    $"Expected {expectedOrder}, but got {uniqueOrders[i]}. Order values: {string.Join(", ", orders)}"
                );
            }
        }

        return ErrOrNothing.Nothing;
    }

    public int Count => Items.Length;
    public static QuestionAnswersList<T> Empty() => new([]);

    public override IEnumerable<object> GetEqualityComponents() => Items;

    public QuestionAnswersList<T> ApplyForEach(Func<T, T> func) =>
        new(Items.Select(func).ToImmutableArray());

    public IEnumerable<TOutput> Select<TOutput>(Func<T, TOutput> func) =>
        Items.Select(func);

    public IEnumerable<BaseQuestionAnswer> AsIEnumerable =>
        Items.Select(a => (BaseQuestionAnswer)a);

    public bool All(Func<T, bool> func) =>
        Items.All(func);
}