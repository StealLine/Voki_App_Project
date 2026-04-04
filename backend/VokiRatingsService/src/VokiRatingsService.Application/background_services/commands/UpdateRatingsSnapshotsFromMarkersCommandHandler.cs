using Microsoft.Extensions.Logging;
using SharedKernel;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Application.background_services.commands;

public record UpdateRatingsSnapshotsFromMarkersCommand() : ICommand<UpdateRatingsSnapshotsFromMarkersCommandResult>
{
    bool ICommand<UpdateRatingsSnapshotsFromMarkersCommandResult>.RequireTransaction => false;
}

internal class UpdateRatingsSnapshotsFromMarkersCommandHandler :
    ICommandHandler<UpdateRatingsSnapshotsFromMarkersCommand, UpdateRatingsSnapshotsFromMarkersCommandResult>
{
    private readonly IUpdateRatingsSnapshotMarkerRepository _updateRatingsSnapshotMarkerRepository;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IVokiRatingsSnapshotRepository _vokiRatingsSnapshotRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateRatingsSnapshotsFromMarkersCommandHandler(
        IUpdateRatingsSnapshotMarkerRepository updateRatingsSnapshotMarkerRepository,
        IRatingsRepository ratingsRepository,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _updateRatingsSnapshotMarkerRepository = updateRatingsSnapshotMarkerRepository;
        _ratingsRepository = ratingsRepository;
        _vokiRatingsSnapshotRepository = vokiRatingsSnapshotRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<UpdateRatingsSnapshotsFromMarkersCommandResult>> Handle(
        UpdateRatingsSnapshotsFromMarkersCommand request, CancellationToken ct
    ) {
        HashSet<VokiId> vokiIds = await _updateRatingsSnapshotMarkerRepository.GetIdsOfMarkedVokis(100, ct);
        DateTime now = _dateTimeProvider.UtcNow;

        if (vokiIds.Count == 0) {
            return new UpdateRatingsSnapshotsFromMarkersCommandResult(0, 0, now, 0);
        }

        List<VokiRatingsSnapshot> snapshotsToUpdate = new();
        List<VokiRatingsSnapshot> snapshotsToAdd = new();
        Dictionary<VokiId, VokiRatingsDistribution> distributions =
            await _ratingsRepository.GetRatingsDistributionForVokis(vokiIds, ct);
        Dictionary<VokiId, VokiRatingsSnapshot> vokiIdToLastSnapshot =
            await _vokiRatingsSnapshotRepository.GetLastSnapshotForVokisAsTracking(vokiIds, ct);

        foreach (var vokiId in vokiIds) {
            VokiRatingsDistribution newDistribution = distributions.GetValueOrDefault(vokiId, VokiRatingsDistribution.Empty);
            if (vokiIdToLastSnapshot.TryGetValue(vokiId, out var lastVokiSnapshot) && lastVokiSnapshot.IsInSameDayAs(now)) {
                lastVokiSnapshot.Update(now, newDistribution);
                snapshotsToUpdate.Add(lastVokiSnapshot);
            }
            else {
                var newSnapshot = VokiRatingsSnapshot.CreateNew(vokiId, now, newDistribution);
                snapshotsToAdd.Add(newSnapshot);
            }
        }

        await _vokiRatingsSnapshotRepository.UpdateRange(snapshotsToUpdate, ct);
        await _vokiRatingsSnapshotRepository.AddRange(snapshotsToAdd, ct);


        await _updateRatingsSnapshotMarkerRepository.ExecuteDeleteByVokiIds(vokiIds, ct);

        return new UpdateRatingsSnapshotsFromMarkersCommandResult(
            SnapshotsUpdatedCount: snapshotsToUpdate.Count,
            NewSnapshotsCount: snapshotsToAdd.Count,
            now,
            MarkersCount: vokiIds.Count()
        );
    }
}

public record UpdateRatingsSnapshotsFromMarkersCommandResult(
    int SnapshotsUpdatedCount,
    int NewSnapshotsCount,
    DateTime Time,
    int MarkersCount
);