using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.voki_taken_records;

internal class VokiTakenQuestionDetailsArrayConverter : ValueConverter<ImmutableArray<VokiTakenQuestionDetails>, string[]>
{
    public VokiTakenQuestionDetailsArrayConverter() : base(
        details => ToStringArray(details),
        strings => FromStringArray(strings)
    ) { }

    private const char Sep = '|';

    private static string[] ToStringArray(ImmutableArray<VokiTakenQuestionDetails> details) =>
        details.Select(DetailToString).ToArray();

    private static string DetailToString(VokiTakenQuestionDetails d) =>
        $"{d.QuestionId}{Sep}{d.OrderInVokiTaking}{Sep}{AnswerIdsToString(d.ChosenAnswerIds)}";

    private static string AnswerIdsToString(ImmutableHashSet<GeneralVokiAnswerId> answers) =>
        string.Join(',', answers.Select(a => a.ToString()));

    private static ImmutableArray<VokiTakenQuestionDetails> FromStringArray(string[] strs) {
        List<VokiTakenQuestionDetails> details = new(strs.Length);
        foreach (var str in strs) {
            var parts = str.Split(Sep);
            if (parts.Length != 3) {
                UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    $"Wrong number of parts in VokiTakenQuestionDetails string: {str}"
                ));
            }

            GeneralVokiQuestionId questionId = new(new(parts[0]));
            var orderInVokiTaking = QuestionOrderInVokiTakingSession.Create( ushort.Parse(parts[1])).AsSuccess();
            ImmutableHashSet<GeneralVokiAnswerId> chosenAnswerIds = parts[2]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(aId => new GeneralVokiAnswerId(new(aId)))
                .ToImmutableHashSet();

            details.Add(new VokiTakenQuestionDetails(questionId, chosenAnswerIds, orderInVokiTaking));
        }

        return details.ToImmutableArray();
    }
}

internal class VokiTakenQuestionDetailsArrayComparer : ValueComparer<ImmutableArray<VokiTakenQuestionDetails>>
{
    public VokiTakenQuestionDetailsArrayComparer() : base(
        (t1, t2) => t1.SequenceEqual(t2!, EqualityComparer<VokiTakenQuestionDetails>.Default),
        t => t.Select(x => x!.GetHashCode()).Aggregate(0, (x, y) => x ^ y),
        t => t.ToImmutableArray()
    ) { }
}