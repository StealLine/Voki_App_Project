using InfrastructureShared.Base.domain_events_publisher;
using SharedKernel.domain;

namespace DbSeeder;

internal class FakePublisher : IDomainEventsPublisher
{
    private FakePublisher() { }
    public Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public static readonly FakePublisher Instance = new ();
}