using ApiShared;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace AlbumsService.Api.contracts;

public class RemoveVokiFromAlbumRequest : IRequestWithValidationNeeded
{
    public string VokiId { get; init; }

    public ErrOrNothing Validate() {
        if (Guid.TryParse(VokiId, out var guid)) {
            ParsedVokiId = new(guid);
            return ErrOrNothing.Nothing;
        }

        return ErrFactory.IncorrectFormat("Could not parse Voki Id", $"{VokiId} is an incorrect voki id");
    }

    public VokiId ParsedVokiId { get; private set; }
}