using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class VokisRepository : IVokisRepository
{
    private readonly VokisCatalogDbContext _db;

    public VokisRepository(VokisCatalogDbContext db)
    {
        _db = db;
    }

    public Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(
        AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct
    ) => _db.Database
        .SqlQuery<Guid>($@"
            SELECT ""Id"" AS ""Value""
            FROM ""BaseVokis""
            WHERE ""PrimaryAuthorId"" = {authenticatedUserCtx.UserId.Value}
               OR {authenticatedUserCtx.UserId.Value} = ANY(""CoAuthorIds"")
            ORDER BY ""PublicationDate"" DESC
        ")
        .Select(id => new VokiId(id))
        .ToArrayAsync(cancellationToken: ct);


    public Task<Voki?> GetById(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<Voki[]> GetAllSorted(CancellationToken ct) => _db.Vokis
        .OrderByDescending(v => v.PublicationDate)
        .ToArrayAsync(cancellationToken: ct);

    public Task<Voki[]> GetMultipleById(VokiId[] queryVokiIds, CancellationToken ct) =>
        _db.Vokis
            .Where(v => queryVokiIds.Contains(v.Id))
            .ToArrayAsync(cancellationToken: ct);

    public Task<Voki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct) =>
        _db.FindByIdForUpdateAsync<Voki, VokiId>(vokiId, ct);

    public async Task Add(Voki voki, CancellationToken ct)
    {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Update(Voki voki, CancellationToken ct)
    {
        _db.ThrowIfDetached(voki);
        _db.Vokis.Update(voki);
        await _db.SaveChangesAsync(ct);
    }
}