using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record class AlbumWithVokiIdsResponse(
    string Id,
    string Name,
    string Icon,
    string MainColor,
    string SecondaryColor,
    string[] VokiIds
) : ICreatableResponse<VokiAlbum>
{
    public static ICreatableResponse<VokiAlbum> Create(VokiAlbum a) => new AlbumWithVokiIdsResponse(
        a.Id.ToString(),
        a.Name.ToString(),
        a.Icon.ToString(),
        a.MainColor.ToString(),
        a.SecondaryColor.ToString(),
        a.VokiIds.Select(v => v.ToString()).ToArray()
    );
}