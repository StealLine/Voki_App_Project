using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) : IQuery<Voki>;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, Voki>
{
    private readonly IVokisRepository _vokisRepository;

    public GetVokiQueryHandler(IVokisRepository vokisRepository) {
        _vokisRepository = vokisRepository;
    }

    public async Task<ErrOr<Voki>> Handle(GetVokiQuery query, CancellationToken ct) {
        Voki? voki = await _vokisRepository.GetById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested voki was not found",
                $"There is no published voki with id {query.VokiId}"
            );
        }
        return voki;
    }
}