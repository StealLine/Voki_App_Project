using AlbumsService.Application.voki_albums.queries;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record ListAlbumsDataForVokiResponse(
    AlbumDataForVokiResponse[] Albums
) : ICreatableResponse<AlbumWithVokiPresenceDto[]>
{
    public static ICreatableResponse<AlbumWithVokiPresenceDto[]> Create(AlbumWithVokiPresenceDto[] albums) =>
        new ListAlbumsDataForVokiResponse(
            albums
                .Select(AlbumDataForVokiResponse.Create)
                .ToArray()
        );
}

public record AlbumDataForVokiResponse(
    string Id,
    string Name,
    string Icon,
    string MainColor,
    string SecondaryColor,
    DateTime CreationDate,
    bool IsVokiInAlbum
)
{
    public static AlbumDataForVokiResponse Create(AlbumWithVokiPresenceDto dto) => new(
        dto.Id.ToString(),
        dto.Name.ToString(),
        dto.Icon.ToString(),
        dto.MainColor.ToString(),
        dto.SecondaryColor.ToString(),
        dto.CreationDate,
        dto.IsVokiInAlbum
    );
}