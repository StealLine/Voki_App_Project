using SharedKernel.domain;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.update_ratings_snapshot_marker_aggregate;
using VokiRatingsService.Domain.voki_rating_aggregate.events;

namespace VokiRatingsService.Application.voki_ratings.domain_event_handlers;

internal class VokiRatingsSetChangedEventHandler : IDomainEventHandler<VokiRatingsSetChangedEvent>
{
    private readonly IUpdateRatingsSnapshotMarkerRepository _updateRatingsSnapshotMarkerRepository;

    public VokiRatingsSetChangedEventHandler(
        IUpdateRatingsSnapshotMarkerRepository updateRatingsSnapshotMarkerRepository
    )
    {
        _updateRatingsSnapshotMarkerRepository = updateRatingsSnapshotMarkerRepository;
    }

    public async Task Handle(VokiRatingsSetChangedEvent e, CancellationToken ct)
    {
        bool exists = await _updateRatingsSnapshotMarkerRepository.ExistsForVoki(e.VokiId, ct);
        if (!exists)
        {
            var marker = UpdateRatingsSnapshotMarker.CreateNew(e.VokiId);
            await _updateRatingsSnapshotMarkerRepository.Add(marker, ct);
        }
    }
}