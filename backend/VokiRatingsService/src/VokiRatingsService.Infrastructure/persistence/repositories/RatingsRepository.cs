using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class RatingsRepository : IRatingsRepository
{
    private readonly VokiRatingsDbContext _db;

    public RatingsRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public Task<VokiRating?> GetUserRatingForVokiForUpdate(
        AuthenticatedUserCtx userContext, VokiId vokiId, CancellationToken ct
    ) =>
        _db.FindForUpdateAsync<VokiRating>(
            r =>
                r.VokiId == vokiId
                && r.UserId == userContext.UserId,
            ct
        );

    public Task<VokiRating[]> ListRatingsForVoki(VokiId vokiId, CancellationToken ct) => _db.Ratings
        .Where(r => r.VokiId == vokiId)
        .ToArrayAsync(cancellationToken: ct);

    public async Task Update(VokiRating rating, CancellationToken ct) {
        _db.ThrowIfDetached(rating);
        _db.Ratings.Update(rating);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Add(VokiRating rating, CancellationToken ct) {
        _db.Ratings.Add(rating);
        await _db.SaveChangesAsync(ct);
    }

    public Task<VokiIdWithCurrentRatingDto[]> ListIdsOfVokiRatedByUser(
        AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct
    ) =>
        _db.Ratings
            .Where(r => r.UserId == authenticatedUserCtx.UserId)
            .Select(r => new VokiIdWithCurrentRatingDto(r.VokiId, r.CurrentValue, r.LastUpdated))
            .ToArrayAsync(ct);

    public async Task<VokiRatingsDistribution> GetRatingsDistributionForVoki(VokiId vokiId, CancellationToken ct) {
        RatingValue[] ratings = await _db.Ratings
            .Where(r => r.VokiId == vokiId)
            .Select(r => r.CurrentValue)
            .ToArrayAsync(ct);

        return VokiRatingsDistribution.FromRatingsArray(ratings);
    }

    public async Task<Dictionary<VokiId, VokiRatingsDistribution>> GetRatingsDistributionForVokis(
        IEnumerable<VokiId> vokiIds,
        CancellationToken ct
    ) {
        var ids = vokiIds.ToArray();

        var grouped = await _db.Ratings
            .Where(r => ids.Contains(r.VokiId))
            .GroupBy(r => r.VokiId)
            .Select(g => new {
                VokiId = g.Key,
                Ratings = g.Select(x => x.CurrentValue).ToArray()
            })
            .ToListAsync(ct);

        return grouped.ToDictionary(
            x => x.VokiId,
            x => VokiRatingsDistribution.FromRatingsArray(x.Ratings)
        );
    }
}