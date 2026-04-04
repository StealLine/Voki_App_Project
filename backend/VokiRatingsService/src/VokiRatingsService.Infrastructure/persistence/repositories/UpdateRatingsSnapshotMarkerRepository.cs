using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.update_ratings_snapshot_marker_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class UpdateRatingsSnapshotMarkerRepository : IUpdateRatingsSnapshotMarkerRepository
{
    private readonly VokiRatingsDbContext _db;

    public UpdateRatingsSnapshotMarkerRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public async Task Add(UpdateRatingsSnapshotMarker marker, CancellationToken ct) {
        _db.UpdateRatingsSnapshotMarkers.Add(marker);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsForVoki(VokiId vokiId, CancellationToken ct) =>
        _db.UpdateRatingsSnapshotMarkers.AnyAsync(m => m.VokiId == vokiId, ct);


    public Task<HashSet<VokiId>> GetIdsOfMarkedVokis(int limit, CancellationToken ct) =>
        _db.UpdateRatingsSnapshotMarkers
            .Select(m => m.VokiId)
            .Take(limit)
            .ToHashSetAsync(ct);

    public async Task ExecuteDeleteByVokiIds(IEnumerable<VokiId> markers, CancellationToken ct) =>
        await _db.UpdateRatingsSnapshotMarkers
            .Where(m => markers.Contains(m.VokiId))
            .ExecuteDeleteAsync(ct);
}