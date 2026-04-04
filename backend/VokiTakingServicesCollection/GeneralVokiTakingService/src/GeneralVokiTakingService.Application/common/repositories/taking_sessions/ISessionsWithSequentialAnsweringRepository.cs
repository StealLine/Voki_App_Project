using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.common.repositories.taking_sessions;

public interface ISessionsWithSequentialAnsweringRepository
{
    Task<SessionWithSequentialAnswering?> GetByIdForUpdate(VokiTakingSessionId sessionId, CancellationToken ct);
    Task Delete(SessionWithSequentialAnswering question, CancellationToken ct);

    Task Update(SessionWithSequentialAnswering session, CancellationToken ct);
}