using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.voki_albums.queries;

public sealed record ListAlbumsDataForVokiQuery(
    VokiId VokiId
) : IQuery<AlbumWithVokiPresenceDto[]>,
    IWithAuthCheckStep;

internal sealed class ListAlbumsDataForVokiQueryHandler :
    IQueryHandler<ListAlbumsDataForVokiQuery, AlbumWithVokiPresenceDto[]>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListAlbumsDataForVokiQueryHandler(IVokiAlbumsRepository vokiAlbumsRepository, IUserCtxProvider userCtxProvider) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<AlbumWithVokiPresenceDto[]>> Handle(
        ListAlbumsDataForVokiQuery query, CancellationToken ct
    ) {
        VokiAlbum[] albums = await _vokiAlbumsRepository.ListAlbumsForUser((_userCtxProvider.Current as AuthenticatedUserCtx)!, ct);
        return albums
            .Select(a => AlbumWithVokiPresenceDto.FromAlbum(a, query.VokiId))
            .ToArray();
    }
}

public record AlbumWithVokiPresenceDto(
    VokiAlbumId Id,
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor,
    DateTime CreationDate,
    bool IsVokiInAlbum
)
{
    public static AlbumWithVokiPresenceDto FromAlbum(VokiAlbum album, VokiId vokiId) => new(
        album.Id,
        album.Name,
        album.Icon,
        album.MainColor,
        album.SecondaryColor,
        album.CreationDate,
        album.HasVoki(vokiId)
    );
}