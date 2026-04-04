using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.free_answering;

public class FinishVokiTakingWithFreeAnsweringRequest : IRequestWithValidationNeeded
{
    public string SessionId { get; init; }
    public Dictionary<string, string[]> ChosenAnswers { get; init; }
    public DateTime ClientSessionStartTime { get; init; }
    public DateTime ServerSessionStartTime { get; init; }
    public DateTime ClientSessionFinishTime { get; init; }

    private const int
        MaxQuestionsCount = 500,
        MaxAnswersInQuestionCount = 500;

    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SessionId) || !Guid.TryParse(SessionId, out var sessionGuid)) {
            return ErrFactory.IncorrectFormat("Provided session id is invalid");
        }

        ParsedSessionId = new VokiTakingSessionId(sessionGuid);

        if (ClientSessionFinishTime < ClientSessionStartTime) {
            return ErrFactory.Conflict("Client finish time cannot be earlier than client start time");
        }

        if (ChosenAnswers.Count > MaxQuestionsCount) {
            return ErrFactory.ValueOutOfRange();
        }

        ParsedChosenAnswers = new Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>(ChosenAnswers.Count);

        foreach (var (questionIdStr, answersArray) in ChosenAnswers) {
            if (string.IsNullOrWhiteSpace(questionIdStr) || !Guid.TryParse(questionIdStr, out var questionGuid)) {
                return ErrFactory.IncorrectFormat("One or more of provided question id is invalid");
            }

            if (answersArray is null) {
                return ErrFactory.NoValue.Common();
            }

            if (answersArray.Length > MaxAnswersInQuestionCount) {
                return ErrFactory.ValueOutOfRange();
            }

            var parsedAnswers = answersArray
                .Where(id => Guid.TryParse(id, out _))
                .Select(g => new GeneralVokiAnswerId(new(g)))
                .ToImmutableHashSet();


            ParsedChosenAnswers.Add(new(questionGuid), parsedAnswers);
        }

        return ErrOrNothing.Nothing;
    }

    public VokiTakingSessionId ParsedSessionId { get; private set; }
    public Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> ParsedChosenAnswers { get; private set; }
    public ClientServerTimePairDto ParsedSessionStartTime =>
        new(Client: ServerSessionStartTime, Server: ServerSessionStartTime);
}