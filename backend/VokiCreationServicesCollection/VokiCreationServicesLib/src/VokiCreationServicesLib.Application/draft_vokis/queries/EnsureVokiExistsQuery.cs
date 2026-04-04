using VokiCreationServicesLib.Application.common;

namespace VokiCreationServicesLib.Application.draft_vokis.queries;

public sealed record EnsureVokiExistsQuery(
    VokiId VokiId
) :
    IQuery<VokiId>;

internal sealed class EnsureVokiExistsQueryHandler : IQueryHandler<EnsureVokiExistsQuery, VokiId>
{
    private readonly IDraftVokiRepository _draftGeneralVokisRepository;


    public EnsureVokiExistsQueryHandler(IDraftVokiRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiId>> Handle(EnsureVokiExistsQuery query, CancellationToken ct) {
        bool anyVoki = await _draftGeneralVokisRepository.AnyVokiWithId(query.VokiId, ct);
        if (!anyVoki) {
            return ErrFactory.NotFound.Voki("There is no such draft general voki");
        }

        return query.VokiId;
    }
}