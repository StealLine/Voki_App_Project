using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using SharedKernel.domain;
using SharedKernel.exceptions;

namespace InfrastructureShared.EfCore;

public static class DbGuard
{
    public static void ThrowIfDetached(
        this DbContext db,
        IAggregateRoot entity,
        [CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    ) {
        if (db.Entry(entity).State == EntityState.Detached) {
            UnexpectedBehaviourException.ThrowErr(
                err: ErrFactory.ProgramBug(
                    $"Detached entity '{entity.GetType().FullName}' passed to '{caller}' method. File: {callerFilePath}. Line number: {callerLineNumber}"
                ),
                userMessage: "Something went wrong while processing your request. Please try again late",
                caller: caller
            );
        }
    }

    public static void ThrowIfDetached(
        this DbContext db,
        IEnumerable<IAggregateRoot> entities,
        [CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    ) {
        IList<IAggregateRoot> materialized =
            entities as IList<IAggregateRoot>
            ?? entities.ToList();

        var errEntity = materialized.FirstOrDefault(
            e => db.Entry(e).State == EntityState.Detached
        );

        if (errEntity is not null) {
            ThrowIfDetached(
                db,
                errEntity,
                caller: caller,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber
            );
        }
    }


    public static void ThrowIfDetached(
        this DbContext db,
        IAggregateRoot entity,
        string userMessage,
        [CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    ) {
        if (db.Entry(entity).State == EntityState.Detached) {
            UnexpectedBehaviourException.ThrowErr(
                err: ErrFactory.ProgramBug(
                    $"Detached entity '{entity.GetType().FullName}' passed to {caller}. Line number: {callerLineNumber}"
                ),
                userMessage: userMessage,
                caller: caller
            );
        }
    }
}