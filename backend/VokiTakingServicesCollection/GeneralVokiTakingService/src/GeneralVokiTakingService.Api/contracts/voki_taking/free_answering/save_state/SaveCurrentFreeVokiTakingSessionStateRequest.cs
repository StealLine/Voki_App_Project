using GeneralVokiTakingService.Domain.common;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.free_answering.save_state;

public class SaveCurrentFreeVokiTakingSessionStateRequest : IRequestWithValidationNeeded
{
    public string SessionId { get; init; }
    public Dictionary<string, string[]> QuestionIdToChosenAnswers { get; init; }

    private const int
        MaxQuestionsCount = 500,
        MaxAnswersInQuestionCount = 500;

    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SessionId) || !Guid.TryParse(SessionId, out var sessionGuid)) {
            return ErrFactory.IncorrectFormat("Provided session id is invalid");
        }

        ParsedSessionId = new VokiTakingSessionId(sessionGuid);

        if (QuestionIdToChosenAnswers.Count > MaxQuestionsCount) {
            return ErrFactory.ValueOutOfRange("Answers data provided for too many questions");
        }

        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> parsed = new(QuestionIdToChosenAnswers.Count);
        foreach (var (questionIdStr, answersArray) in QuestionIdToChosenAnswers) {
            if (string.IsNullOrWhiteSpace(questionIdStr) || !Guid.TryParse(questionIdStr, out var questionGuid)) {
                return ErrFactory.IncorrectFormat($"One or more of provided question id is invalid. Id: {questionIdStr}");
            }

            if (answersArray is null) {
                continue;
            }

            if (answersArray.Length > MaxAnswersInQuestionCount) {
                return ErrFactory.ValueOutOfRange();
            }

            var parsedAnswers = answersArray
                .Where(id => Guid.TryParse(id, out _))
                .Select(g => new GeneralVokiAnswerId(new(g)))
                .ToImmutableHashSet();


            parsed.Add(new(questionGuid), parsedAnswers);
        }

        ParsedChosenAnswers = parsed.ToImmutableDictionary();

        return ErrOrNothing.Nothing;
    }

    public VokiTakingSessionId ParsedSessionId { get; private set; }

    public ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>
        ParsedChosenAnswers { get; private set; }
}