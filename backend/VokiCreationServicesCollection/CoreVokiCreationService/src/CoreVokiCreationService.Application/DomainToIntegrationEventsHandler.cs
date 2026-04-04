using ApplicationShared;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.common.vokis;
using SharedKernel.domain;
using SharedKernel.integration_events.draft_vokis;
using SharedKernel.integration_events.draft_vokis.co_authors;
using SharedKernel.integration_events.draft_vokis.new_voki_initialized;

namespace CoreVokiCreationService.Application;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<NewDraftVokiInitializedEvent>,
    IDomainEventHandler<CoAuthorInviteAcceptedEvent>,
    IDomainEventHandler<VokiCoAuthorRemovedEvent>,
    IDomainEventHandler<DraftVokiExpectedManagersUpdatedEvent>

// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(NewDraftVokiInitializedEvent e, CancellationToken ct) {
        await e.Type.Match(
            onGeneral: async () => await _integrationEventPublisher.Publish(
                new GeneralDraftVokiInitializedIntegrationEvent(
                    e.VokiId, e.PrimaryAuthorId, e.Name.ToString(), e.Cover.ToString(), e.CreationDate
                ), ct),
            onTierList: async () => await _integrationEventPublisher.Publish(
                new TierListDraftVokiInitializedIntegrationEvent(), ct),
            onScoring: async () => await _integrationEventPublisher.Publish(
                new ScoringDraftVokiInitializedIntegrationEvent(), ct)
        );
    }

    public async Task Handle(CoAuthorInviteAcceptedEvent e, CancellationToken ct) => await
        _integrationEventPublisher.Publish(new DraftVokiNewCoAuthorAddedIntegrationEvent(
            e.VokiId, e.AppUserId, e.VokiType, e.UserIdsExpectedToBecomeManagers.ToArray()
        ), ct);


    public async Task Handle(VokiCoAuthorRemovedEvent e, CancellationToken ct) => await
        _integrationEventPublisher.Publish(new DraftVokiCoAuthorRemovedIntegrationEvent(
            e.VokiId, e.AppUserId, e.VokiType, e.UserIdsExpectedToBecomeManagers.ToArray()
        ), ct);

    public async Task Handle(DraftVokiExpectedManagersUpdatedEvent e, CancellationToken ct) => await
        _integrationEventPublisher.Publish(new DraftVokiExpectedManagersUpdatedIntegrationEvent(
         e.VokiId, e.VokiType, e.ExpectedManagers.ToArray()
        ), ct);
}