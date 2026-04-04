using VokiRatingsService.Application.vokis.queries;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Api.contracts.manage_voki;

public record ManageVokiRatingsOverviewResponse(
    ManageVokiRatingsOverviewResponse.ManageVokiRatingsDailySnapshotResponse[] Snapshots,
    DateTime VokiPublicationDate
) : ICreatableResponse<ManageVokiRatingsOverviewQueryResult>
{
    public static ICreatableResponse<ManageVokiRatingsOverviewQueryResult> Create(
        ManageVokiRatingsOverviewQueryResult res
    ) => new ManageVokiRatingsOverviewResponse(
        res.Snapshots.Select(ManageVokiRatingsDailySnapshotResponse.FromSnapshot).ToArray(),
        res.VokiPublicationDate
    );

    public record ManageVokiRatingsDailySnapshotResponse(
        DateTime Date,
        Dictionary<ushort, uint> Distribution
    )
    {
        public static ManageVokiRatingsDailySnapshotResponse FromSnapshot(VokiRatingsSnapshot sn) => new(
            sn.Date,
            new() {
                [1] = sn.Distribution.Rating1Count,
                [2] = sn.Distribution.Rating2Count,
                [3] = sn.Distribution.Rating3Count,
                [4] = sn.Distribution.Rating4Count,
                [5] = sn.Distribution.Rating5Count,
            }
        );
    }
}