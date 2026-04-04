using SharedKernel.common.vokis;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

public record class VokiNameUpdatedEvent(
    VokiId VokiId,
    VokiName NewName
) : IDomainEvent;