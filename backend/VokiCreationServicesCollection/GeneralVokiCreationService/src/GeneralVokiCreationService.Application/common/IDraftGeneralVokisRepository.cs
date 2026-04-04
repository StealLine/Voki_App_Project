using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.common;

public interface IDraftGeneralVokisRepository : IDraftVokiRepository
{
    Task<DraftGeneralVoki?> GetById(VokiId generalVokiId, CancellationToken ct);

    new Task<DraftGeneralVoki?> GetByIdForUpdate(VokiId generalVokiId, CancellationToken ct);

    async Task<BaseDraftVoki?> IDraftVokiRepository.GetByIdForUpdate(VokiId vokiId, CancellationToken ct) =>
        await GetById(vokiId, ct);

    Task Add(DraftGeneralVoki voki, CancellationToken ct);
    Task Update(DraftGeneralVoki generalVoki, CancellationToken ct);
    Task Delete(DraftGeneralVoki voki, CancellationToken ct);

    //with question
    Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId, CancellationToken ct);

    Task<DraftGeneralVoki?> GetWithQuestionsForUpdate(VokiId vokiId, CancellationToken ct);

    //with question and results
    Task<DraftGeneralVoki?> GetWithQuestionsAndResults(VokiId queryVokiId, CancellationToken ct);
    Task<DraftGeneralVoki?> GetWithQuestionsAndResultsForUpdate(VokiId vokiId, CancellationToken ct);
    //with results

    Task<DraftGeneralVoki?> GetWithResults(VokiId vokiId, CancellationToken ct);
    Task<DraftGeneralVoki?> GetWithResultsForUpdate(VokiId vokiId, CancellationToken ct);
}