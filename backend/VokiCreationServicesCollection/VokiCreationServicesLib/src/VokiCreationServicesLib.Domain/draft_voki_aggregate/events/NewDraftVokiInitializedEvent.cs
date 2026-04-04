namespace VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

public record class NewDraftVokiInitializedEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId
) : IDomainEvent;