using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Application.vokis.queries;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Application.vokis.commands;

public sealed record TakeVokiRatingsSnapshotCommand(
    VokiId VokiId
) : ICommand<ManageVokiRatingsOverviewQueryResult>,
    IWithAuthCheckStep;

internal sealed class TakeVokiRatingsSnapshotCommandHandler
    : ICommandHandler<TakeVokiRatingsSnapshotCommand, ManageVokiRatingsOverviewQueryResult>
{
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IVokisRepository _vokisRepository;
    private readonly IVokiRatingsSnapshotRepository _ratingsSnapshotRepository;

    public TakeVokiRatingsSnapshotCommandHandler(
        IRatingsRepository ratingsRepository,
        IDateTimeProvider dateTimeProvider,
        IUserCtxProvider userCtxProvider,
        IVokisRepository vokisRepository,
        IVokiRatingsSnapshotRepository ratingsSnapshotRepository
    ) {
        _ratingsRepository = ratingsRepository;
        _dateTimeProvider = dateTimeProvider;
        _userCtxProvider = userCtxProvider;
        _vokisRepository = vokisRepository;
        _ratingsSnapshotRepository = ratingsSnapshotRepository;
    }


    public async Task<ErrOr<ManageVokiRatingsOverviewQueryResult>> Handle(
        TakeVokiRatingsSnapshotCommand command, CancellationToken ct
    ) {
        Voki? voki = await _vokisRepository.GetVokiById(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki does not exist");
        }


        if (!voki.CanUserManage(command.UserCtx(_userCtxProvider))) {
            return ErrFactory.NoAccess("To get this data you need to be a Voki manager");
        }

        DateTime now = _dateTimeProvider.UtcNow;
        VokiRatingsDistribution distribution = await _ratingsRepository.GetRatingsDistributionForVoki(command.VokiId, ct);
        VokiRatingsSnapshot? snapshot = await _ratingsSnapshotRepository.GetLastSnapshotForVokiForUpdate(command.VokiId, ct);
        if (snapshot is not null && snapshot.IsInSameDayAs(now)) {
            snapshot.Update(now, distribution);
            await _ratingsSnapshotRepository.Update(snapshot, ct);
        }
        else {
            snapshot = VokiRatingsSnapshot.CreateNew(command.VokiId, now, distribution);
            await _ratingsSnapshotRepository.Add(snapshot, ct);
        }

        VokiRatingsSnapshot[] snapshots = await _ratingsSnapshotRepository.ListSortedSnapshotsForVoki(command.VokiId, ct);
        return new ManageVokiRatingsOverviewQueryResult(snapshots, voki.PublicationDate);
    }
}