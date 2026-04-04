namespace GeneralVokiTakingService.Api.contracts.voki_taking;

public interface IVokiTakingSessionResponse
{
    public string VokiId { get; }
    public string SessionId { get; }
    public ushort TotalQuestionsCount { get; }
    public DateTime StartedAt { get; }
}

public interface IExistingUnfinishedVokiTakingSessionResponse : IVokiTakingSessionResponse
{
    public ushort QuestionsWithSavedAnswersCount { get; }
}