using ApplicationShared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InfrastructureShared.EfCore.unit_of_work;

internal sealed class DbTransactionBasedUnitOfWorkManager<TDbContext>
    : IUnitOfWorkManager
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public DbTransactionBasedUnitOfWorkManager(TDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<IUnitOfWork> Start(CancellationToken ct) {
        IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(ct);
        return new DbTransactionBasedUnitOfWork(transaction);
    }
}