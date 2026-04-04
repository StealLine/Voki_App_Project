using MassTransit;
using SharedKernel;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.voki_publishing;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Application.vokis.integration_event_handlers;

public class BaseVokiPublishedIntegrationEventHandler : IConsumer<BaseVokiPublishedIntegrationEvent>
{
    private readonly IVokisRepository _vokisRepository;
    private readonly IVokiRatingsSnapshotRepository _vokiRatingsSnapshotRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BaseVokiPublishedIntegrationEventHandler(
        IVokisRepository vokisRepository,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _vokisRepository = vokisRepository;
        _vokiRatingsSnapshotRepository = vokiRatingsSnapshotRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Consume(ConsumeContext<BaseVokiPublishedIntegrationEvent> context) {
        Voki voki = new Voki(
            context.Message.VokiId,
            context.Message.PrimaryAuthorId,
            VokiManagersIdsSet.Create(context.Message.Managers.ToImmutableHashSet()).AsSuccess(),
            context.Message.PublicationDate
        );
        VokiRatingsSnapshot snapshot = VokiRatingsSnapshot.CreateNew(
            context.Message.VokiId,
            _dateTimeProvider.UtcNow,
            VokiRatingsDistribution.Empty
        );

        await _vokisRepository.Add(voki, context.CancellationToken);
        await _vokiRatingsSnapshotRepository.Add(snapshot, context.CancellationToken);
    }
}