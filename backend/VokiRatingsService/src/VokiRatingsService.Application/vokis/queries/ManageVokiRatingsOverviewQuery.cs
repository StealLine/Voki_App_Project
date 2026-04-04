using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Application.vokis.queries;

public sealed record ManageVokiRatingsOverviewQuery(
    VokiId VokiId
) : IQuery<ManageVokiRatingsOverviewQueryResult>,
    IWithAuthCheckStep;

internal sealed class ManageVokiRatingsOverviewQueryHandler :
    IQueryHandler<ManageVokiRatingsOverviewQuery, ManageVokiRatingsOverviewQueryResult>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IVokisRepository _vokisRepository;
    private readonly IVokiRatingsSnapshotRepository _ratingsSnapshotRepository;

    public ManageVokiRatingsOverviewQueryHandler(
        IUserCtxProvider userCtxProvider,
        IVokisRepository vokisRepository,
        IVokiRatingsSnapshotRepository ratingsSnapshotRepository
    ) {
        _userCtxProvider = userCtxProvider;
        _vokisRepository = vokisRepository;
        _ratingsSnapshotRepository = ratingsSnapshotRepository;
    }


    public async Task<ErrOr<ManageVokiRatingsOverviewQueryResult>> Handle(
        ManageVokiRatingsOverviewQuery query, CancellationToken ct
    ) {
        Voki? voki = await _vokisRepository.GetVokiById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki does not exist");
        }


        if (!voki.CanUserManage(query.UserCtx(_userCtxProvider))) {
            return ErrFactory.NoAccess("To get this data you need to be a Voki manager");
        }

        VokiRatingsSnapshot[] snapshots = await _ratingsSnapshotRepository.ListSortedSnapshotsForVoki(query.VokiId, ct);

        return new ManageVokiRatingsOverviewQueryResult(snapshots, voki.PublicationDate);
    }
}

public sealed record ManageVokiRatingsOverviewQueryResult(
    VokiRatingsSnapshot[] Snapshots,
    DateTime VokiPublicationDate
);