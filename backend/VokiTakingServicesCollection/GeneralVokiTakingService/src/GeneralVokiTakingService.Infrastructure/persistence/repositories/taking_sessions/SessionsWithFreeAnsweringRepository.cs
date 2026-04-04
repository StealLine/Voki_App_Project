using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

internal class SessionsWithFreeAnsweringRepository : ISessionsWithFreeAnsweringRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public SessionsWithFreeAnsweringRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public Task<SessionWithFreeAnswering?> GetByIdForUpdate(VokiTakingSessionId sessionId, CancellationToken ct) =>
        _db.FindByIdForUpdateAsync<SessionWithFreeAnswering, VokiTakingSessionId>(sessionId, ct);

    public async Task Delete(SessionWithFreeAnswering session, CancellationToken ct) {
        _db.ThrowIfDetached(session);
        _db.VokiTakingSessionsWithFreeAnswering.Remove(session);
        await _db.SaveChangesAsync(ct);
    }

    public async  Task Update(SessionWithFreeAnswering session, CancellationToken ct) {
        _db.ThrowIfDetached(session);
        _db.VokiTakingSessionsWithFreeAnswering.Update(session);
        await _db.SaveChangesAsync(ct);
    }
}