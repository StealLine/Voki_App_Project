using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class VokisRepository : IVokisRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokisRepository(VokiRatingsDbContext db) {
        _db = db;
    }


    public async Task<VokiManagersDto?> GetVokiManagerDto(VokiId vokiId, CancellationToken ct) {
        var res = await _db.Vokis
            .Select(x => new { x.Id, x.PrimaryAuthorId, x.ManagersSet })
            .FirstOrDefaultAsync(x => x.Id == vokiId, ct);
        if (res is null) {
            return null;
        }

        return new VokiManagersDto(res.Id, res.PrimaryAuthorId, res.ManagersSet);
    }

    public Task<Voki?> GetVokiById(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .FirstOrDefaultAsync(x => x.Id == vokiId, ct);

    public async Task Add(Voki voki, CancellationToken ct) {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }
}