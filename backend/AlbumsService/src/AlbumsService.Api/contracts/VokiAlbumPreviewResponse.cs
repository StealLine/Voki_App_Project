using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record VokiAlbumPreviewResponse(
    string Id,
    string Name,
    string Icon,
    string MainColor,
    string SecondaryColor,
    int VokisCount
) : ICreatableResponse<VokiAlbum>
{
    public static VokiAlbumPreviewResponse FromAlbum(VokiAlbumPreviewDto a) => new(
        a.Id.ToString(),
        a.Name.ToString(),
        a.Icon.ToString(),
        a.MainColor.ToString(),
        a.SecondaryColor.ToString(),
        a.VokiIdsCount
    );

    public static ICreatableResponse<VokiAlbum> Create(VokiAlbum a) => new VokiAlbumPreviewResponse(a.Id.ToString(),
        a.Name.ToString(),
        a.Icon.ToString(),
        a.MainColor.ToString(),
        a.SecondaryColor.ToString(),
        a.VokiIds.Count
    );
}