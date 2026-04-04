using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.voki_albums.queries;

public sealed record GetVokiAlbumToViewQuery(
    VokiAlbumId AlbumId
) : IQuery<VokiAlbum>,
    IWithAuthCheckStep;

internal sealed class GetVokiAlbumToViewQueryHandler : IQueryHandler<GetVokiAlbumToViewQuery, VokiAlbum>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetVokiAlbumToViewQueryHandler(IVokiAlbumsRepository vokiAlbumsRepository, IUserCtxProvider userCtxProvider) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiAlbum>> Handle(GetVokiAlbumToViewQuery query, CancellationToken ct) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetById(query.AlbumId, ct);
        if (album is null) {
            return ErrFactory.NotFound.Common("Requested album not found");
        }

        if (!album.IsUserOwner(_userCtxProvider.Current)) {
            return ErrFactory.NoAccess("Cannot access this album because user is not owner");
        }

        return album;
    }
}