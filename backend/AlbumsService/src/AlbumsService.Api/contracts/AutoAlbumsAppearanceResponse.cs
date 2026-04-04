using AlbumsService.Domain.app_user_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record AutoAlbumsAppearanceResponse(
    AutoAlbumsColorsPairResponse TakenVokisAlbums,
    AutoAlbumsColorsPairResponse RatedVokisAlbums,
    AutoAlbumsColorsPairResponse CommentedVokisAlbums
) : ICreatableResponse<UserAutoAlbumsAppearance>
{
    public static AutoAlbumsAppearanceResponse FromUserAppearance(UserAutoAlbumsAppearance a) => new(
        AutoAlbumsColorsPairResponse.TakenVokisAlbums(a),
        AutoAlbumsColorsPairResponse.RatedVokisAlbums(a),
        AutoAlbumsColorsPairResponse.CommentedVokisAlbums(a)
    );

    public static ICreatableResponse<UserAutoAlbumsAppearance> Create(UserAutoAlbumsAppearance appearance) =>
        FromUserAppearance(appearance);
}

public record AutoAlbumsColorsPairResponse(string MainColor, string SecondaryColor)
{
    public static AutoAlbumsColorsPairResponse TakenVokisAlbums(UserAutoAlbumsAppearance data) =>
        new(data.TakenMainColor.ToString(), data.TakenSecondaryColor.ToString());

    public static AutoAlbumsColorsPairResponse RatedVokisAlbums(UserAutoAlbumsAppearance data) =>
        new(data.RatedMainColor.ToString(), data.RatedSecondaryColor.ToString());

    public static AutoAlbumsColorsPairResponse CommentedVokisAlbums(UserAutoAlbumsAppearance data) =>
        new(data.CommentedMainColor.ToString(), data.CommentedSecondaryColor.ToString());
}