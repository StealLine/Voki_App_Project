namespace VokiRatingsService.Domain.update_ratings_snapshot_marker_aggregate;

public class UpdateRatingsSnapshotMarker : AggregateRoot<UpdateRatingsSnapshotMarkerId>
{
    private UpdateRatingsSnapshotMarker() { }

    public VokiId VokiId { get; }

    private UpdateRatingsSnapshotMarker(UpdateRatingsSnapshotMarkerId id, VokiId vokiId) {
        Id = id;
        VokiId = vokiId;
    }

    public static UpdateRatingsSnapshotMarker CreateNew(VokiId vokiId) {
        return new UpdateRatingsSnapshotMarker(UpdateRatingsSnapshotMarkerId.CreateNew(), vokiId);
    }
}