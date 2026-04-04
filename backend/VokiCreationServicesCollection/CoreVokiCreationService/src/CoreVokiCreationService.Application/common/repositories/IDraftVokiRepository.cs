using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.common.repositories;

public interface IDraftVokiRepository
{
    Task Add(DraftVoki voki, CancellationToken ct);
    Task<VokiId[]> ListVokiAuthoredByUserOrderByCreationDate(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task<DraftVoki?> GetById(VokiId vokiId, CancellationToken ct);
    Task<DraftVoki[]> ListVokisWithCurrentUserAsInvitedForCoAuthor(AuthenticatedUserCtx userContext, CancellationToken ct);
    Task<DraftVoki[]> GetMultipleById(VokiId[] queryVokiIds, CancellationToken ct);
    Task<DraftVoki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct);
    Task Update(DraftVoki voki, CancellationToken ct);
    Task Delete(DraftVoki voki, CancellationToken ct);
}