using AlbumsService.Application.app_users.queries;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record class AllAlbumsPreviewResponse(
    AutoAlbumsAppearanceResponse AutoAlbumsAppearance,
    VokiAlbumPreviewResponse[] Albums
) : ICreatableResponse<ListAllUserAlbumsPreviewQueryResult>
{
    public static ICreatableResponse<ListAllUserAlbumsPreviewQueryResult> Create(
        ListAllUserAlbumsPreviewQueryResult res
    ) => new AllAlbumsPreviewResponse(
        AutoAlbumsAppearanceResponse.FromUserAppearance(res.AutoAlbumsAppearance),
        res.Albums.Select(VokiAlbumPreviewResponse.FromAlbum).ToArray()
    );
}