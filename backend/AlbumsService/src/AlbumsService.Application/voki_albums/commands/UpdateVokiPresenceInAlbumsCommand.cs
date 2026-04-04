using AlbumsService.Application.common.repositories;
using AlbumsService.Application.voki_albums.queries;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record UpdateVokiPresenceInAlbumsCommand(
    VokiId VokiId,
    Dictionary<VokiAlbumId, bool> AlbumIdToIsChosen
) : ICommand<AlbumWithVokiPresenceDto[]>,
    IWithAuthCheckStep;

internal sealed class UpdateVokiPresenceInAlbumsCommandHandler :
    ICommandHandler<UpdateVokiPresenceInAlbumsCommand, AlbumWithVokiPresenceDto[]>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public UpdateVokiPresenceInAlbumsCommandHandler(IUserCtxProvider userCtxProvider,
        IVokiAlbumsRepository vokiAlbumsRepository) {
        _userCtxProvider = userCtxProvider;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOr<AlbumWithVokiPresenceDto[]>> Handle(
        UpdateVokiPresenceInAlbumsCommand command, CancellationToken ct
    ) {
        var aCtx = command.UserCtx(_userCtxProvider);

        Dictionary<VokiAlbumId, VokiAlbum> albums =
            (await _vokiAlbumsRepository.ListUsersAlbumsForUpdate(aCtx, ct))
            .ToDictionary(a => a.Id, a => a);

        List<VokiAlbum> changedAlbumsList = [];
        ErrOrNothing errs = ErrOrNothing.Nothing;

        foreach (var (albumId, isChosen) in command.AlbumIdToIsChosen) {
            if (albums.TryGetValue(albumId, out var album)) {
                ErrOrNothing res = album.SetVokiPresenceTo(aCtx, isChosen, command.VokiId);
                errs.AddNextIfErr(res);
                changedAlbumsList.Add(album);
            }
        }

        if (errs.IsErr(out var err)) {
            if (errs.Any(e => e.Code == ErrCodes.NoAccess)) {
                return ErrFactory.NoAccess(
                    "Couldn't update voki in albums presence because you don't have access to modify some of the albums"
                );
            }

            return err;
        }

        await _vokiAlbumsRepository.UpdateRange(changedAlbumsList, ct);
        return albums.Values
            .Select(a => AlbumWithVokiPresenceDto.FromAlbum(a, command.VokiId))
            .ToArray();
    }
}