using ApplicationShared;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace InfrastructureShared.EfCore.unit_of_work;


internal sealed class DbTransactionBasedUnitOfWork : IUnitOfWork
{
    private readonly IDbContextTransaction _transaction;
    private bool _completed;

    public DbTransactionBasedUnitOfWork(
        IDbContextTransaction transaction
    ) {
        _transaction = transaction;
        Id = transaction.TransactionId;
    }

    public Guid Id { get; }

    public async Task Commit(CancellationToken ct) {
        if (_completed) {
            throw new InvalidOperationException("Unit of work is already completed");
        }

        try {
            await _transaction.CommitAsync(ct);
            _completed = true;
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.LockNotAvailable) {
            await SafeRollback(ct);
            throw new UnitOfWorkLockNotAvailableException();
        }
    }

    public async Task Rollback() {
        if (_completed) {
            return;
        }

        await SafeRollback(CancellationToken.None);
    }

    private async Task SafeRollback(CancellationToken ct) {
        try {
            await _transaction.RollbackAsync(ct);
        }
        finally {
            _completed = true;
        }
    }

    public async ValueTask DisposeAsync() {
        if (!_completed) {
            try {
                await _transaction.RollbackAsync(CancellationToken.None);
            }
            catch {
                // swallow - dispose path must not throw 
            }
        }

        await _transaction.DisposeAsync();
    }
}