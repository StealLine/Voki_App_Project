namespace InfrastructureShared.EfCore;

public sealed class DetachedEntityOperationException : Exception
{
    public DetachedEntityOperationException(Type entityType, string operation)
        : base(
            $"Detached entity of type '{entityType.Name}' was passed to '{operation}'. " +
            "This is a programming error: the entity must be tracked by the DbContext."
        )
    {
        EntityType = entityType;
        Operation = operation;
    }

    public Type EntityType { get; }
    public string Operation { get; }
}
