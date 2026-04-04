namespace VokiRatingsService.Domain;

public class RatingHistoryId(Guid value) : GuidBasedId(value)
{
    public static RatingHistoryId CreateNew() => new(Guid.CreateVersion7());
}
public class VokiRatingsSnapshotId(Guid value) : GuidBasedId(value)
{
    public static VokiRatingsSnapshotId CreateNew() => new(Guid.CreateVersion7());
}
public class UpdateRatingsSnapshotMarkerId(Guid value) : GuidBasedId(value)
{
    public static UpdateRatingsSnapshotMarkerId CreateNew() => new(Guid.CreateVersion7());
}
