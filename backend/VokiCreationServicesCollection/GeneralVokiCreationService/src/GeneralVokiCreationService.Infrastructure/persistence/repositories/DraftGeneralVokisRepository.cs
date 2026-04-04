using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftGeneralVokisRepository : IDraftGeneralVokisRepository
{
    private readonly GeneralVokiCreationDbContext _db;

    public DraftGeneralVokisRepository(GeneralVokiCreationDbContext db) {
        _db = db;
    }


    public async Task Add(DraftGeneralVoki voki, CancellationToken ct) {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }


    public Task<DraftGeneralVoki?> GetById(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);


    public Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .WithQuestions()
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);


    public Task<DraftGeneralVoki?> GetWithQuestionsForUpdate(VokiId vokiId, CancellationToken ct) =>
        _db.FindWithIncludesForUpdateAsync<DraftGeneralVoki, VokiId>(
            includes: q => q.WithQuestions(),
            vokiId, ct
        );


    public Task<DraftGeneralVoki?> GetWithResults(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .WithResults()
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);


    public Task<DraftGeneralVoki?> GetWithResultsForUpdate(VokiId vokiId, CancellationToken ct) =>
        _db.FindWithIncludesForUpdateAsync<DraftGeneralVoki, VokiId>(q => q.WithResults(), vokiId, ct);


    public Task<DraftGeneralVoki?> GetWithQuestionsAndResultsForUpdate(
        VokiId vokiId, CancellationToken ct
    ) => _db.FindWithIncludesForUpdateAsync<DraftGeneralVoki, VokiId>(
        q => q.WithQuestions().AsSplitQuery().WithResults(),
        vokiId, ct
    );


    public Task<DraftGeneralVoki?> GetWithQuestionsAndResults(
        VokiId vokiId, CancellationToken ct
    ) =>
        _db.Vokis
            .WithQuestions()
            .AsSplitQuery()
            .WithResults()
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);


    public Task Delete(DraftGeneralVoki voki, CancellationToken ct) {
        _db.ThrowIfDetached(voki);
        _db.Vokis.Remove(voki);
        return _db.SaveChangesAsync(ct);
    }

    public Task<bool> AnyVokiWithId(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis.AnyAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetByIdForUpdate(
        VokiId generalVokiId, CancellationToken ct
    ) =>
        _db.FindByIdForUpdateAsync<DraftGeneralVoki, VokiId>(generalVokiId, ct);


    async Task<BaseDraftVoki?> IDraftVokiRepository.GetByIdForUpdate(
        VokiId vokiId, CancellationToken ct
    ) =>
        await GetByIdForUpdate(generalVokiId: vokiId, ct);


    public async Task<(VokiName, AppUserId PrimaryAuthor, VokiCoAuthorIdsSet CoAuthors)?> GetVokiName(
        VokiId vokiId, CancellationToken ct
    ) {
        var res = await _db.Vokis
            .Select(v => new {
                v.Id,
                v.Name,
                v.PrimaryAuthorId,
                CoAuthors = EF.Property<VokiCoAuthorIdsSet>(v, "CoAuthors")
            })
            .FirstOrDefaultAsync(v => v.Id == vokiId, ct);

        if (res is null) {
            return null;
        }

        return (res.Name, res.PrimaryAuthorId, res.CoAuthors);
    }


    public async Task Update(DraftGeneralVoki generalVoki, CancellationToken ct) {
        _db.ThrowIfDetached(generalVoki);
        _db.Vokis.Update(generalVoki);
        await _db.SaveChangesAsync(ct);
    }


    async Task IDraftVokiRepository.Update(BaseDraftVoki voki, CancellationToken ct) {
        _db.ThrowIfDetached(voki);

        if (voki is not DraftGeneralVoki generalVoki) {
            UnexpectedBehaviourException.ThrowErr(
                ErrFactory.Unspecified(
                    $"Unexpected type of voki: {voki.GetType()}. Expected: {typeof(DraftGeneralVoki)}"
                )
            );
            throw new();
        }

        await Update(generalVoki: generalVoki, ct);
    }
}

file static class DraftGeneralVokiQueryExtensions
{
    public static IQueryable<DraftGeneralVoki> WithQuestions(this IQueryable<DraftGeneralVoki> query) =>
        query.Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"));


    public static IQueryable<DraftGeneralVoki> WithResults(this IQueryable<DraftGeneralVoki> query) =>
        query.Include(v => EF.Property<List<VokiResult>>(v, "_results"));
}