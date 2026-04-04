using System.Collections.Immutable;

namespace SharedKernel.domain;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : IEntityId
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public IImmutableList<IDomainEvent> GetDomainEventsCopy() => _domainEvents.ToImmutableList();

    public List<IDomainEvent> PopAndClearDomainEvents() {
        if (_domainEvents is null) {
            return [];
        }
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
}

public interface IAggregateRoot
{
    public List<IDomainEvent> PopAndClearDomainEvents();
}