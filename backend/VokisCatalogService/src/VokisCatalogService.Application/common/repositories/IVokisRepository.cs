using SharedKernel.user_ctx;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.common.repositories;

public interface IVokisRepository
{
    Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct);
    Task<Voki?> GetById(VokiId vokiId, CancellationToken ct);
    Task<Voki[]> GetAllSorted(CancellationToken ct);
    Task<Voki[]> GetMultipleById(VokiId[] queryVokiIds, CancellationToken ct);
    Task<Voki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct);
    Task Add(Voki voki, CancellationToken ct);
    Task Update(Voki voki, CancellationToken ct);
}