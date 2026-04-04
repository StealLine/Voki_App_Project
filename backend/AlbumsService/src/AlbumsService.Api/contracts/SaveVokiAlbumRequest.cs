using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;
using SharedKernel.common;
using SharedKernel.errs;

namespace AlbumsService.Api.contracts;

public class SaveVokiAlbumRequest : IRequestWithValidationNeeded
{
    public string Name { get; init; }
    public string Icon { get; init; }
    public string MainColor { get; init; }
    public string SecondaryColor { get; init; }

    public ErrOrNothing Validate() =>
        AlbumName.CheckForErr(Name)
            .WithNextIfErr(AlbumIcon.CheckForErr(Icon))
            .WithNextIfErr(HexColor.CheckHexColorForErr(MainColor))
            .WithNextIfErr(HexColor.CheckHexColorForErr(SecondaryColor));

    public AlbumName ParsedName => AlbumName.Create(Name).AsSuccess();
    public AlbumIcon ParsedIcon => AlbumIcon.Create(Icon).AsSuccess();
    public HexColor ParsedMainColor=> HexColor.Create(MainColor).AsSuccess();
    public HexColor ParsedSecondaryColor => HexColor.Create(SecondaryColor).AsSuccess();
}