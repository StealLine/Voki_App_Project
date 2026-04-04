using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate.events;

namespace VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

public class VokiRatingsSnapshot : AggregateRoot<VokiRatingsSnapshotId>
{
    private VokiRatingsSnapshot() { }
    public VokiId VokiId { get; }
    public DateTime Date { get; private set; }
    public VokiRatingsDistribution Distribution { get; private set; }

    private VokiRatingsSnapshot(VokiRatingsSnapshotId id, VokiId vokiId, DateTime now, VokiRatingsDistribution distribution) {
        Id = id;
        VokiId = vokiId;
        Date = now;
        Distribution = distribution;
    }

    public static VokiRatingsSnapshot CreateNew(VokiId vokiId, DateTime now, VokiRatingsDistribution distribution) {
        VokiRatingsSnapshot sn = new (VokiRatingsSnapshotId.CreateNew(), vokiId, now, distribution);
        if (sn.Distribution.TotalCount != 0) {
            sn.AddDomainEvent(new VokiRatingsChangedCount(vokiId, sn.Distribution.TotalCount));
        }
        return sn;
    }

    public void Update(DateTime now, VokiRatingsDistribution distribution) {
        var oldRatingsCount = this.Distribution.TotalCount;
        Date = now;
        Distribution = distribution;

        var newRatingsCount = this.Distribution.TotalCount;
        if (oldRatingsCount != newRatingsCount) {
            AddDomainEvent(new VokiRatingsChangedCount(VokiId, newRatingsCount));
        }
    }

    public bool IsInSameDayAs(DateTime dateTime) =>
        DateOnly.FromDateTime(this.Date) == DateOnly.FromDateTime(dateTime.Date);
}