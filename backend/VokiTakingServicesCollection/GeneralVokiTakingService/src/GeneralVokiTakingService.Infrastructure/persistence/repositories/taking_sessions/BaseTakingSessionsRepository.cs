using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

internal class BaseTakingSessionsRepository : IBaseTakingSessionsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public BaseTakingSessionsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task Add(BaseVokiTakingSession session, CancellationToken ct) {
        await _db.BaseVokiTakingSessions.AddAsync(session, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<(VokiTakingSessionId Id, bool IsWithForceSequentialAnswering)?> GetUserUnfinishedSessionBriefData(
        VokiId commandVokiId, AuthenticatedUserCtx aUserCtx, CancellationToken ct
    ) {
        var res = await _db.BaseVokiTakingSessions
            .Select(s => new { s.Id, s.VokiId, s.VokiTaker, s.IsWithForceSequentialAnswering })
            .FirstOrDefaultAsync(s => s.VokiId == commandVokiId && s.VokiTaker == aUserCtx.UserId, ct);
        if (res is null) {
            return null;
        }

        return (res.Id, res.IsWithForceSequentialAnswering);
    }


    public Task<BaseVokiTakingSession?> GetForVokiAndUser(
        VokiId vokiId, AuthenticatedUserCtx aUserCtx, CancellationToken ct
    ) {
        return _db.BaseVokiTakingSessions
            .FirstOrDefaultAsync(s => s.VokiId == vokiId && s.VokiTaker == aUserCtx.UserId, ct);
    }

    public Task<BaseVokiTakingSession?> GetById(VokiTakingSessionId sessionId, CancellationToken ct) =>
        _db.BaseVokiTakingSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId, ct);
}