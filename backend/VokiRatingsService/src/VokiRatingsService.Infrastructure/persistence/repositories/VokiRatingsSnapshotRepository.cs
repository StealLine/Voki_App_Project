using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class VokiRatingsSnapshotRepository : IVokiRatingsSnapshotRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokiRatingsSnapshotRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public async Task<Dictionary<VokiId, VokiRatingsSnapshot>> GetLastSnapshotForVokisAsTracking(
        IEnumerable<VokiId> vokiIds, CancellationToken ct
    ) {
        var ids = vokiIds.ToArray();

        var snapshots = await _db.VokiRatingsSnapshots
            .Where(x => ids.Contains(x.VokiId))
            .GroupBy(x => x.VokiId)
            .Select(g => g
                .OrderByDescending(x => x.Date)
                .First())
            .AsTracking()
            .ToListAsync(ct);

        return snapshots.ToDictionary(x => x.VokiId);
    }

    public Task<VokiRatingsSnapshot?> GetLastSnapshotForVokiForUpdate(VokiId vokiId, CancellationToken ct) =>
        _db.FindForUpdateAsync<VokiRatingsSnapshot>(
            x => x.VokiId == vokiId,
            ct,
            orderBy: q => q.OrderByDescending(x => x.Date)
        );

    public async Task Update(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        _db.ThrowIfDetached(snapshot);
        _db.VokiRatingsSnapshots.Update(snapshot);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateRange(IEnumerable<VokiRatingsSnapshot> snapshots, CancellationToken ct) {
        VokiRatingsSnapshot[] materialized = snapshots.ToArray();

        _db.ThrowIfDetached(materialized);
        _db.VokiRatingsSnapshots.UpdateRange(materialized);
        await _db.SaveChangesAsync(ct);
    }

    public async Task AddRange(IEnumerable<VokiRatingsSnapshot> snapshots, CancellationToken ct) {
        await _db.VokiRatingsSnapshots.AddRangeAsync(snapshots, ct);
        await _db.SaveChangesAsync(ct);
    }


    public async Task Add(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        await _db.VokiRatingsSnapshots.AddAsync(snapshot, ct);
        await _db.SaveChangesAsync(ct);
    }


    public Task<VokiRatingsSnapshot[]> ListSortedSnapshotsForVoki(VokiId vokiId, CancellationToken ct) =>
        _db.VokiRatingsSnapshots
            .Where(s => s.VokiId == vokiId)
            .OrderBy(s => s.Date)
            .ToArrayAsync(ct);
}