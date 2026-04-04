using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.dtos;

public record SavedUnfinishedVokiTakingSessionDto(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    DateTime StartedAt,
    ushort QuestionsWithSavedAnswersCount,
    ushort TotalQuestionsCount
)
{
    public static SavedUnfinishedVokiTakingSessionDto Create(BaseVokiTakingSession takingSession) => new(
        takingSession.VokiId,
        takingSession.Id,
        takingSession.StartTime,
        takingSession.QuestionsWithSavedAnswersCount(),
        takingSession.TotalQuestionsCount
    );
}