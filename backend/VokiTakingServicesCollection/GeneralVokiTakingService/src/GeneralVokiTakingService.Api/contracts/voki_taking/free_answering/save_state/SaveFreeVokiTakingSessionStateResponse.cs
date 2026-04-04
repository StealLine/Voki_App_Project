using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.free_answering;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.free_answering.save_state;

public record SaveFreeVokiTakingSessionStateResponse(
    Dictionary<string, string[]> SavedChosenAnswers
) : ICreatableResponse<SessionWithFreeAnsweringSavedState>
{
    public static ICreatableResponse<SessionWithFreeAnsweringSavedState> Create(SessionWithFreeAnsweringSavedState state) =>
        new SaveFreeVokiTakingSessionStateResponse(
            state.QuestionsWithAnswers.ToDictionary(
                kvp => kvp.Key.ToString(),
                kvp => kvp.Value.Select(aId => aId.ToString()).ToArray()
            )
        );
}