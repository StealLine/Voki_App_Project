using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record CopyVokisFromAlbumsToAlbumCommand(
    VokiAlbumId AlbumId,
    ImmutableHashSet<VokiAlbumId> AlbumIdsToCopyFrom
) :
    ICommand<VokisToAlbumFromAlbumsCopied>,
    IWithBasicValidationStep,
    IWithAuthCheckStep
{
    public ErrOrNothing Validate() {
        if (AlbumIdsToCopyFrom.Count == 0) {
            return ErrFactory.NoValue.Common("Albums to copy from are not specified");
        }

        if (AlbumIdsToCopyFrom.Count > VokiAlbum.MaxAlbumsToCopyFrom) {
            return ErrFactory.LimitExceeded(
                $"Too many source albums specified. Maximum allowed: {VokiAlbum.MaxAlbumsToCopyFrom}",
                $"Provided: {AlbumIdsToCopyFrom.Count}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}

internal sealed class CopyVokisFromAlbumsToAlbumCommandHandler
    : ICommandHandler<CopyVokisFromAlbumsToAlbumCommand, VokisToAlbumFromAlbumsCopied>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public CopyVokisFromAlbumsToAlbumCommandHandler(IUserCtxProvider userCtxProvider,
        IVokiAlbumsRepository vokiAlbumsRepository) {
        _userCtxProvider = userCtxProvider;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOr<VokisToAlbumFromAlbumsCopied>> Handle(
        CopyVokisFromAlbumsToAlbumCommand command, CancellationToken ct
    ) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetByIdForUpdate(command.AlbumId, ct);
        if (album is null) {
            return ErrFactory.NotFound.Common("Could not copy vokis because destination does not exist");
        }

        var albumsToCopyFrom = await _vokiAlbumsRepository.ListByIds(command.AlbumIdsToCopyFrom, ct);
        ErrOr<VokisToAlbumFromAlbumsCopied> copyRes = album.CopyVokisFromAlbums(
            command.UserCtx(_userCtxProvider),
            albumsToCopyFrom
        );
        if (copyRes.IsErr(out var err)) {
            return err;
        }

        await _vokiAlbumsRepository.Update(album, ct);
        return copyRes.AsSuccess();
    }
}