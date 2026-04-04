using SharedKernel.common.vokis;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.common;

public interface IDraftVokiRepository
{
    Task<bool> AnyVokiWithId(VokiId queryVokiId, CancellationToken ct);
    Task<BaseDraftVoki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct);
    Task<(VokiName, AppUserId PrimaryAuthor, VokiCoAuthorIdsSet CoAuthors )?> GetVokiName(VokiId vokiId, CancellationToken ct);
    Task Update(BaseDraftVoki voki, CancellationToken ct);
}