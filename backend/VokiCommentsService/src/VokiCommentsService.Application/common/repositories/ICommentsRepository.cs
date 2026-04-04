using SharedKernel.user_ctx;

namespace VokiCommentsService.Application.common.repositories;

public interface ICommentsRepository
{
    Task<VokiIdWithLastCommentedDateDto[]> OrderedIdsOfVokiCommentedByUser(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
}

public record VokiIdWithLastCommentedDateDto(VokiId VokiId, DateTime Date);