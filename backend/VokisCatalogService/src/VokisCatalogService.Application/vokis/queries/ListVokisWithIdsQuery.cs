using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListVokisWithIdsQuery(VokiId[] VokiIds) :
    IQuery<Voki[]>;

internal sealed class ListVokisWithIdsQueryHandler : IQueryHandler<ListVokisWithIdsQuery, Voki[]>
{
    private readonly IVokisRepository _vokisRepository;

    public ListVokisWithIdsQueryHandler(IVokisRepository vokisRepository) {
        _vokisRepository = vokisRepository;
    }

    public async Task<ErrOr<Voki[]>> Handle(ListVokisWithIdsQuery withIdsQuery, CancellationToken ct) {
        return await _vokisRepository.GetMultipleById(withIdsQuery.VokiIds, ct);
    }
}