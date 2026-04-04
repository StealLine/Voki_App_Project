using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.common.repositories.taking_sessions;

public interface ISessionsWithFreeAnsweringRepository
{
    Task<SessionWithFreeAnswering?> GetByIdForUpdate(VokiTakingSessionId sessionId, CancellationToken ct);
    Task Delete(SessionWithFreeAnswering session, CancellationToken ct);
    Task Update(SessionWithFreeAnswering session, CancellationToken ct);
}