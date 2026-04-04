using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.sequential_answering;

public class FinishVokiTakingWithSequentialAnsweringRequest : IRequestWithValidationNeeded
{
    public string SessionId { get; init; }
    public string LastQuestionId { get; init; }
    public ushort LastQuestionOrderInVokiTaking { get; init; }
    public Dictionary<string, bool> LastQuestionAnswersWithIsChosen { get; init; }


    public DateTime ClientLastQuestionShownAt { get; init; }
    public DateTime ServerLastQuestionShownAt { get; init; }
    public DateTime ClientLastQuestionAnsweredAt { get; init; }

    public DateTime ClientSessionStartTime { get; init; }
    public DateTime ServerSessionStartTime { get; init; }
    public DateTime ClientSessionFinishTime { get; init; }

    private const int MaxAnswersInQuestionCount = 500;

    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SessionId) || !Guid.TryParse(SessionId, out var sessionGuid)) {
            return ErrFactory.IncorrectFormat("Provided session id is invalid");
        }

        ParsedSessionId = new VokiTakingSessionId(sessionGuid);

        if (string.IsNullOrWhiteSpace(LastQuestionId) || !Guid.TryParse(LastQuestionId, out var questionGuid)) {
            return ErrFactory.IncorrectFormat("Provided last question id is invalid");
        }

        var orderRes = QuestionOrderInVokiTakingSession.Create(LastQuestionOrderInVokiTaking);
        if (orderRes.IsErr(out var err)) {
            return err;
        }

        ParsedLastQuestionOrder = orderRes.AsSuccess();
        ParsedLastQuestionId = new GeneralVokiQuestionId(questionGuid);

        if (LastQuestionAnswersWithIsChosen is null || LastQuestionAnswersWithIsChosen.Count == 0) {
            return ErrFactory.NoValue.Common("Chosen answers are missing");
        }

        string[] chosenAnswers = LastQuestionAnswersWithIsChosen
            .Where(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .ToArray();

        if (chosenAnswers.Length > MaxAnswersInQuestionCount) {
            return ErrFactory.ValueOutOfRange();
        }

        ImmutableHashSet<GeneralVokiAnswerId> parsedAnswers = chosenAnswers
            .Where(id => Guid.TryParse(id, out _))
            .Select(id => new GeneralVokiAnswerId(new Guid(id)))
            .ToImmutableHashSet();

        if (parsedAnswers.Count < chosenAnswers.Length) {
            return ErrFactory.IncorrectFormat(
                $"Couldn't parse some of the chosen answers. Chosen: {chosenAnswers.Length}. Parsed: {parsedAnswers.Count}",
                "Try reloading the page"
            );
        }

        ParsedQuestionChosenAnswers = parsedAnswers;

        if (ClientSessionFinishTime < ClientSessionStartTime) {
            return ErrFactory.Conflict("Client finish time cannot be earlier than client start time");
        }


        if (ClientLastQuestionAnsweredAt < ClientLastQuestionShownAt) {
            return ErrFactory.Conflict("Client answered before the question was shown");
        }


        if (LastQuestionOrderInVokiTaking == 0) {
            return ErrFactory.ValueOutOfRange("Last question order cannot be zero or negative");
        }


        return ErrOrNothing.Nothing;
    }

    public VokiTakingSessionId ParsedSessionId { get; private set; }
    public GeneralVokiQuestionId ParsedLastQuestionId { get; private set; }
    public QuestionOrderInVokiTakingSession ParsedLastQuestionOrder { get; private set; }

    public ImmutableHashSet<GeneralVokiAnswerId> ParsedQuestionChosenAnswers { get; private set; }

    public ClientServerTimePairDto ParsedLastQuestionShownAt =>
        new(Client: ClientLastQuestionShownAt, Server: ServerLastQuestionShownAt);

    public ClientServerTimePairDto ParsedSessionStartTime =>
        new(Client: ServerSessionStartTime, Server: ServerSessionStartTime);
}