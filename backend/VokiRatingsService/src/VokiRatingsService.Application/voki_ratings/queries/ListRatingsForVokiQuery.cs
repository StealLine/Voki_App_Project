using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record ListRatingsForVokiQuery(VokiId VokiId) : IQuery<VokiRating[]>;

internal sealed class ListRatingsForVokiQueryHandler : IQueryHandler<ListRatingsForVokiQuery, VokiRating[]>
{
    private readonly IRatingsRepository _ratingsRepository;

    public ListRatingsForVokiQueryHandler(IRatingsRepository ratingsRepository) {
        _ratingsRepository = ratingsRepository;
    }

    public async Task<ErrOr<VokiRating[]>> Handle(
        ListRatingsForVokiQuery query, CancellationToken ct
    ) {
        return await _ratingsRepository.ListRatingsForVoki(query.VokiId, ct);
    }
}