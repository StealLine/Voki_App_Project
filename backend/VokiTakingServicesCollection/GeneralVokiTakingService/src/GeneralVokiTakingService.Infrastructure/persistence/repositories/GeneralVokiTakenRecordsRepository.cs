using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class GeneralVokiTakenRecordsRepository : IGeneralVokiTakenRecordsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public GeneralVokiTakenRecordsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralVokiTakenRecord vokiTakenRecord, CancellationToken ct) {
        await _db.VokiTakenRecords.AddAsync(vokiTakenRecord, ct);
        await _db.SaveChangesAsync(ct);
    }

    public Task<GeneralVokiTakenRecord[]> GetByUserForVoki(
        VokiId vokiId, AuthenticatedUserCtx ctx, CancellationToken ct
    ) => _db.VokiTakenRecords
        .Where(r => r.TakenVokiId == vokiId && r.VokiTakerId == ctx.UserId)
        .ToArrayAsync(ct);

    public async Task<Dictionary<GeneralVokiResultId, uint>> GetResultIdsToCountForVoki(
        VokiId vokiId, CancellationToken ct
    ) => await _db.VokiTakenRecords
        .Where(r => r.TakenVokiId == vokiId)
        .GroupBy(r => r.ReceivedResultId)
        .Select(g => new { ResultId = g.Key, Count = g.Count() })
        .ToDictionaryAsync(x => x.ResultId, x => (uint)x.Count, ct);
}