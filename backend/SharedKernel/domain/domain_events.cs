namespace SharedKernel.domain;

public interface IDomainEvent;
public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T e, CancellationToken ct);
}
public abstract class IDomainToIntegrationEventsHandler;
