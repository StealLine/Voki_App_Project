using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.common.repositories;

public interface IGeneralVokiTakenRecordsRepository
{
    Task Add(GeneralVokiTakenRecord vokiTakenRecord, CancellationToken ct);
    Task<GeneralVokiTakenRecord[]> GetByUserForVoki(VokiId vokiId, AuthenticatedUserCtx ctx, CancellationToken ct);
    Task<Dictionary<GeneralVokiResultId, uint>> GetResultIdsToCountForVoki(VokiId vokiId, CancellationToken ct);
}