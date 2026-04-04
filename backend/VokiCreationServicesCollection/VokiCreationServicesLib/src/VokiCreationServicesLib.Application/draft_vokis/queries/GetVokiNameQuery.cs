using ApplicationShared;
using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.common.vokis;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.user_ctx;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.draft_vokis.queries;

public sealed record GetVokiNameQuery(VokiId VokiId) :
    IQuery<VokiName>,
    IWithAuthCheckStep;

internal sealed class GetVokiNameQueryHandler : IQueryHandler<GetVokiNameQuery, VokiName>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetVokiNameQueryHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiName>> Handle(GetVokiNameQuery query, CancellationToken ct) {
        var result = await _draftVokiRepository.GetVokiName(query.VokiId, ct);
        if (result is null) {
            return ErrFactory.NotFound.Voki("Could not get Voki name because Voki doesn't exist");
        }

        var (vokiName, primaryAuthorId, coAuthors) = result.Value;
        if (BaseDraftVoki.HasUserAccess(query.UserCtx(_userCtxProvider), primaryAuthorId, coAuthors)) {
            return vokiName;
        }

        return ErrFactory.NoAccess("Could not get Voki name because user doesn't have access to this Voki");
    }
}