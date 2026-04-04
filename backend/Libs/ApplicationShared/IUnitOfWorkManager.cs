namespace ApplicationShared;

public interface IUnitOfWorkManager
{
    public Task<IUnitOfWork> Start(CancellationToken ct);
}

public interface IUnitOfWork : IAsyncDisposable
{
    public Guid Id { get; }
    public Task Commit(CancellationToken ct);
    public Task Rollback();
}

public class UnitOfWorkLockNotAvailableException : Exception;
