using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record AlbumCreatedResponse(
    string CreatedAlbumId
) : ICreatableResponse<VokiAlbum>
{
    public static ICreatableResponse<VokiAlbum> Create(VokiAlbum album) => new AlbumCreatedResponse(
        album.Id.ToString()
    );
}