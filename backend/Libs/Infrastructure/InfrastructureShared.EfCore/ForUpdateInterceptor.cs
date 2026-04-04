using System.Data.Common;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InfrastructureShared.EfCore;

public sealed class ForUpdateInterceptor : DbCommandInterceptor
{
    private const string ForUpdateTag = $"-- {DbContextLockExtensions.ForUpdateTagValue}";

    private static void TryApplyForUpdate(DbCommand command) {
        if (!IsSelect(command)) {
            return;
        }

        if (command.CommandText.Contains("FOR UPDATE", StringComparison.OrdinalIgnoreCase)) {
            return;
        }


        if (command.CommandText.Contains(ForUpdateTag, StringComparison.Ordinal)) {
            command.CommandText += " FOR UPDATE";
        }
    }

    private static bool IsSelect(DbCommand command) {
        var text = command.CommandText.AsSpan().TrimStart();

        while (text.StartsWith("--", StringComparison.Ordinal)) {
            var newline = text.IndexOf('\n');
            if (newline == -1) {
                return false;
            }

            text = text[(newline + 1)..].TrimStart();
        }

        return text.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result
    ) {
        TryApplyForUpdate(command);
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default
    ) {
        TryApplyForUpdate(command);
        return ValueTask.FromResult(result);
    }
}