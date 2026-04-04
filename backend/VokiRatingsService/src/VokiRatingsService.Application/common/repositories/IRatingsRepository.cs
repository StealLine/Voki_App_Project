using SharedKernel.user_ctx;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IRatingsRepository
{
    Task<VokiRating?> GetUserRatingForVokiForUpdate(AuthenticatedUserCtx userContext, VokiId vokiId, CancellationToken ct);
    Task<VokiRating[]> ListRatingsForVoki(VokiId vokiId, CancellationToken ct);
    Task Update(VokiRating rating, CancellationToken ct);
    Task Add(VokiRating rating, CancellationToken ct);
    Task<VokiIdWithCurrentRatingDto[]> ListIdsOfVokiRatedByUser(AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct);
    Task<VokiRatingsDistribution> GetRatingsDistributionForVoki(VokiId vokiId, CancellationToken ct);

    Task<Dictionary<VokiId, VokiRatingsDistribution>> GetRatingsDistributionForVokis(
        IEnumerable<VokiId> vokiIds,
        CancellationToken ct
    );
}

public record VokiIdWithCurrentRatingDto(VokiId VokiId, RatingValue Value, DateTime DateTime);