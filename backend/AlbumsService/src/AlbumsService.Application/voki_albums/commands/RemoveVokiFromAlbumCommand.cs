using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record RemoveVokiFromAlbumCommand(
    VokiAlbumId AlbumId,
    VokiId VokiId
) : ICommand,
    IWithAuthCheckStep;

internal sealed class RemoveVokiFromAlbumCommandHandler : ICommandHandler<RemoveVokiFromAlbumCommand>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public RemoveVokiFromAlbumCommandHandler(IUserCtxProvider userCtxProvider, IVokiAlbumsRepository vokiAlbumsRepository) {
        _userCtxProvider = userCtxProvider;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOrNothing> Handle(RemoveVokiFromAlbumCommand command, CancellationToken ct) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetByIdForUpdate(command.AlbumId, ct);
        if (album is null) {
            return ErrFactory.NotFound.Common("Could not update the album because it doesn't exist");
        }

        ErrOrNothing res = album.RemoveVoki(command.UserCtx(_userCtxProvider), command.VokiId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _vokiAlbumsRepository.Update(album, ct);
        return ErrOrNothing.Nothing;
    }
}