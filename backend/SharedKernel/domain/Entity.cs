namespace SharedKernel.domain;

public abstract class Entity<IdType> : IComparable where IdType : IEntityId
{
    public IdType Id { get; protected init; } = default!;

    public override bool Equals(object? other) {
        if (other is null || other.GetType() != GetType()) {
            return false;
        }

        Entity<IdType> otherEntity = (Entity<IdType>)other;
        return Id.Equals(otherEntity.Id);
    }

    public override int GetHashCode() =>
        Id.GetHashCode();

    public int CompareTo(object? obj) => (obj is Entity<IdType> entity) ? Id.CompareTo(entity.Id) : -1;
}