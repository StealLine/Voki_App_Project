using AlbumsService.Domain.app_user_aggregate;
using ApiShared;
using SharedKernel.common;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace AlbumsService.Api.contracts;

public class UpdateAutoAlbumsAppearanceRequest : IRequestWithValidationNeeded
{
    public string TakenMainColor { get; init; }
    public string TakenSecondaryColor { get; init; }
    public string RatedMainColor { get; init; }
    public string RatedSecondaryColor { get; init; }
    public string CommentedMainColor { get; init; }
    public string CommentedSecondaryColor { get; init; }

    public ErrOrNothing Validate() {
        var errs = ErrOrNothing.Nothing
            .WithNextIfErr(TryParseColor(TakenMainColor, "taken", "main", out var takenMain))
            .WithNextIfErr(TryParseColor(TakenSecondaryColor, "taken", "secondary", out var takenSecondary))
            .WithNextIfErr(TryParseColor(RatedMainColor, "rated", "main", out var ratedMain))
            .WithNextIfErr(TryParseColor(RatedSecondaryColor, "rated", "secondary", out var ratedSecondary))
            .WithNextIfErr(TryParseColor(CommentedMainColor, "commented", "main", out var commentedMain))
            .WithNextIfErr(TryParseColor(CommentedSecondaryColor, "commented", "secondary", out var commentedSecondary));

        if (errs.IsErr()) {
            return errs;
        }

        ParsedAppearance = new(
            takenMain, takenSecondary,
            ratedMain, ratedSecondary,
            commentedMain, commentedSecondary
        );

        return ErrOrNothing.Nothing;
    }

    private static ErrOrNothing TryParseColor(
        string colorToParse,
        string albumName, string colorName,
        out HexColor color
    ) {
        if (string.IsNullOrEmpty(colorToParse)) {
            color = HexColor.Default;
            return ErrFactory.NoValue.Common($"No {colorName} color provided for {albumName} Vokis album");
        }

        if (HexColor.Create(colorToParse).IsSuccess(out color)) {
            return ErrOrNothing.Nothing;
        }

        return ErrFactory.IncorrectFormat($"Provided {colorName} color for {albumName} Vokis album is invalid");
    }

    public UserAutoAlbumsAppearance ParsedAppearance { get; private set; }
}