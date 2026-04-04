using GeneralVokiTakingService.Application.general_vokis.commands;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.continue_taking;

public record ContinueVokiTakingResponse(
    string VokiId,
    string VokiName,
    bool IsWithForceSequentialAnswering,
    GeneralVokiTakingResponseQuestionData[] Questions,
    string SessionId,
    DateTime StartedAt,
    DateTime ContinuedAt,
    ushort TotalQuestionsCount,
    Dictionary<string, string[]> SavedChosenAnswers,
    string CurrentQuestionId
) : ICreatableResponse<ContinueVokiTakingCommandResult>, IVokiTakingSessionResponse
{
    public static ICreatableResponse<ContinueVokiTakingCommandResult> Create(ContinueVokiTakingCommandResult res) =>
        new ContinueVokiTakingResponse(
            res.SessionData.VokiId.ToString(),
            res.SessionData.VokiName.ToString(),
            res.SessionData.IsWithForceSequentialAnswering,
            res.SessionData.QuestionsToShow.Select(GeneralVokiTakingResponseQuestionData.FromQuestion).ToArray(),
            res.SessionData.SessionId.ToString(),
            res.SessionData.StartedAt,
            res.CurrentDateTime,
            res.SessionData.TotalQuestionsCount,
            res.SavedChosenAnswers.ToDictionary(
                qToAnsw => qToAnsw.Key.ToString(),
                qToAnsw => qToAnsw.Value.Select(a => a.ToString()).ToArray()
            ),
            res.CurrentQuestionId.ToString()
        );
}