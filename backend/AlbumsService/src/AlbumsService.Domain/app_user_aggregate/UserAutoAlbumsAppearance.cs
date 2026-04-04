namespace AlbumsService.Domain.app_user_aggregate;

public record UserAutoAlbumsAppearance(
    HexColor TakenMainColor,
    HexColor TakenSecondaryColor,
    HexColor RatedMainColor,
    HexColor RatedSecondaryColor,
    HexColor CommentedMainColor,
    HexColor CommentedSecondaryColor
)
{
    public static UserAutoAlbumsAppearance Default() => new(
        HexColor.Create("#007dfe").AsSuccess(),
        HexColor.Create("#007dfe").AsSuccess(),
        HexColor.Create("#ffb700").AsSuccess(),
        HexColor.Create("#ffb700").AsSuccess(),
        HexColor.Create("#ff0a67").AsSuccess(),
        HexColor.Create("#ff0a67").AsSuccess()
    );
}