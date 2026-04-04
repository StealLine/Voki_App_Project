using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using InfrastructureShared.EfCore.db_extensions.expression_visitors;
using Microsoft.EntityFrameworkCore;
using SharedKernel.domain;

namespace InfrastructureShared.EfCore.db_extensions;

public static class DbContextLockExtensions
{
    internal const string ForUpdateTagValue = "FOR_UPDATE";

    public static async Task<T?> FindByIdForUpdateAsync<T, TId>(
        this DbContext db,
        TId id,
        CancellationToken ct
    )
        where T : AggregateRoot<TId>
        where TId : ValueObject, IEntityId {
        return await db.Set<T>()
            .AsTracking()
            .TagWith(ForUpdateTagValue)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }


    public static async Task<T?> FindWithIncludesForUpdateAsync<T, TId>(
        this DbContext db,
        Func<IQueryable<T>, IQueryable<T>> includes,
        TId id,
        CancellationToken ct,
        [CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    )
        where T : AggregateRoot<TId>
        where TId : ValueObject, IEntityId {
        IQueryable<T> q = includes(db.Set<T>());
        OnlyIncludesVisitor.EnsureOnlyIncludes(
            q, caller: caller,
            callerFilePath: callerFilePath, callerLineNumber: callerLineNumber
        );

        await db.Set<T>()
            .TagWith(ForUpdateTagValue)
            .Where(e => e.Id == id)
            .Select(_ => 1)
            .FirstOrDefaultAsync(ct);

        return await q
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }


    public static Task<T?> FindForUpdateAsync<T>(
        this DbContext db,
        Expression<Func<T, bool>> findPredicate,
        CancellationToken ct,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
    ) where T : class, IAggregateRoot {
        IQueryable<T> q = db.Set<T>();

        if (orderBy is not null) {
            q = orderBy(q);
        }

        return q.TagWith(ForUpdateTagValue)
            .AsTracking()
            .FirstOrDefaultAsync(findPredicate, ct);
    }


    public static async Task<T[]> ListForUpdateAsync<T>(
        this DbContext db,
        Expression<Func<T, bool>> predicate,
        CancellationToken ct
    ) where T : class, IAggregateRoot {
        return await db.Set<T>()
            .TagWith(ForUpdateTagValue)
            .AsTracking()
            .Where(predicate)
            .ToArrayAsync(cancellationToken: ct);
    }
}