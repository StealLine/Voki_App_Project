using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.queries;


public sealed record ListAllVokisQuery() : IQuery<Voki[]>;

internal sealed class ListAllVokisQueryHandler : IQueryHandler<ListAllVokisQuery, Voki[]>
{
    private readonly IVokisRepository _vokisRepository;

    public ListAllVokisQueryHandler(IVokisRepository vokisRepository) {
        _vokisRepository = vokisRepository;
    }

    public async Task<ErrOr<Voki[]>> Handle(ListAllVokisQuery query, CancellationToken ct) =>
        await _vokisRepository.GetAllSorted(ct);
     
}