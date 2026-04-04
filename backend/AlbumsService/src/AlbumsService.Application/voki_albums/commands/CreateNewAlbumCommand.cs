using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record CreateNewAlbumCommand(
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor
) :
    ICommand<VokiAlbum>,
    IWithAuthCheckStep
{
    public Err UnauthenticatedErr => ErrFactory.AuthRequired("To create albums you need to be logged in");
}

internal sealed class CreateNewAlbumCommandHandler :
    ICommandHandler<CreateNewAlbumCommand, VokiAlbum>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IDateTimeProvider _dateTimeProvider;


    public CreateNewAlbumCommandHandler(
        IUserCtxProvider userCtxProvider, IVokiAlbumsRepository vokiAlbumsRepository,
        IAppUsersRepository appUsersRepository, IDateTimeProvider dateTimeProvider
    ) {
        _userCtxProvider = userCtxProvider;
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _appUsersRepository = appUsersRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<VokiAlbum>> Handle(CreateNewAlbumCommand command, CancellationToken ct) {
        var aCtx = command.UserCtx(_userCtxProvider);

        AppUser? user = await _appUsersRepository.GetCurrentForUpdate(aCtx, ct);
        if (user is null) {
            return ErrFactory.NotFound.User();
        }

        VokiAlbum album = VokiAlbum.CreateNew(
            aCtx, command.Name, command.Icon, command.MainColor, command.SecondaryColor, _dateTimeProvider.UtcNow
        );

        var addingRes = user.AddAlbum(album.Id);
        if (addingRes.IsErr(out var err)) {
            return err;
        }

        await _appUsersRepository.Update(user, ct);
        await _vokiAlbumsRepository.Add(album, ct);

        return album;
    }
}