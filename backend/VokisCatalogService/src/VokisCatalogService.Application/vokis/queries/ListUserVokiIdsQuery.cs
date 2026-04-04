using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokisCatalogService.Application.common.repositories;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListIdsOfVokiAuthoredByUser() :
    IQuery<VokiId[]>,
    IWithAuthCheckStep;

internal sealed class ListIdsOfVokiAuthoredByUserHandler : IQueryHandler<ListIdsOfVokiAuthoredByUser, VokiId[]>
{
    private readonly IVokisRepository _vokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListIdsOfVokiAuthoredByUserHandler(IVokisRepository vokisRepository, IUserCtxProvider userCtxProvider) {
        _vokisRepository = vokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiId[]>> Handle(ListIdsOfVokiAuthoredByUser query, CancellationToken ct)=>
       await _vokisRepository.ListVokiAuthoredByUserIdsOrderByCreationDate(query.UserCtx(_userCtxProvider), ct);
}