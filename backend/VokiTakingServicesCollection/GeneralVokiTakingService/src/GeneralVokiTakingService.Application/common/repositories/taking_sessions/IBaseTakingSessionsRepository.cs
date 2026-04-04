using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.common.repositories.taking_sessions;

public interface IBaseTakingSessionsRepository
{
    public Task Add(BaseVokiTakingSession session, CancellationToken ct);

    Task<(VokiTakingSessionId Id, bool IsWithForceSequentialAnswering)?> GetUserUnfinishedSessionBriefData(
        VokiId commandVokiId, AuthenticatedUserCtx aUserCtx, CancellationToken ct
    );

    Task<BaseVokiTakingSession?> GetForVokiAndUser(VokiId vokiId, AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task<BaseVokiTakingSession?> GetById(VokiTakingSessionId sessionId, CancellationToken ct);
}