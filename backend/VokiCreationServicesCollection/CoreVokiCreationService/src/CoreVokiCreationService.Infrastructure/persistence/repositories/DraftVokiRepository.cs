using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftVokiRepository : IDraftVokiRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public DraftVokiRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }


    public async Task Add(DraftVoki voki, CancellationToken ct) {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }

    public Task<VokiId[]> ListVokiAuthoredByUserOrderByCreationDate(AuthenticatedUserCtx aUserCtx, CancellationToken ct) =>
        _db.Vokis
            .FromSqlInterpolated($@"
                SELECT ""Id""
                FROM ""Vokis""
                WHERE {aUserCtx.UserId.Value} = ""PrimaryAuthorId""
                   OR {aUserCtx.UserId.Value} = ANY(""CoAuthorIds"")
                ORDER BY ""CreationDate"" DESC
            ")
            .Select(v => v.Id)
            .ToArrayAsync(cancellationToken: ct);

    public Task<DraftVoki?> GetById(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftVoki[]> ListVokisWithCurrentUserAsInvitedForCoAuthor(
        AuthenticatedUserCtx userContext, CancellationToken ct
    ) =>
        _db.Vokis
            .FromSqlInterpolated($@"
                SELECT *
                FROM ""Vokis""
                WHERE {userContext.UserId.Value} = ANY(""InvitedForCoAuthorUserIds"")
            ")
            .ToArrayAsync(cancellationToken: ct);


    public Task<DraftVoki[]> GetMultipleById(VokiId[] queryVokiIds, CancellationToken ct) =>
        _db.Vokis
            .Where(v => queryVokiIds.Contains(v.Id))
            .ToArrayAsync(cancellationToken: ct);

    public Task<DraftVoki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct) =>
        _db.FindByIdForUpdateAsync<DraftVoki, VokiId>(vokiId, ct);

    public async Task Update(DraftVoki voki, CancellationToken ct) {
        _db.ThrowIfDetached(voki);
        _db.Vokis.Update(voki);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Delete(DraftVoki voki, CancellationToken ct) {
        _db.ThrowIfDetached(voki);
        _db.Vokis.Remove(voki);
        await _db.SaveChangesAsync(ct);
    }
}