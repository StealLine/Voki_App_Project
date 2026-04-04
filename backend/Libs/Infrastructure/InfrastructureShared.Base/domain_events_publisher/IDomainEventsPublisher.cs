using SharedKernel.domain;

namespace InfrastructureShared.Base.domain_events_publisher;

public interface IDomainEventsPublisher
{
    Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken );
}
