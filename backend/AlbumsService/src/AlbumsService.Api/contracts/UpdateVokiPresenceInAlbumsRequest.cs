using ApiShared;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace AlbumsService.Api.contracts;

public class UpdateVokiPresenceInAlbumsRequest : IRequestWithValidationNeeded
{
    private const int MaxCount = 200;
    public Dictionary<string, bool>? AlbumIdToIsChosen { get; init; } = null;

    public ErrOrNothing Validate() {
        if (AlbumIdToIsChosen is null || AlbumIdToIsChosen.Count == 0) {
            return ErrFactory.NoValue.Common("Voki albums entry is not provided");
        }

        if (AlbumIdToIsChosen.Count >= MaxCount) {
            return ErrFactory.NoValue.Common("Voki albums entry is too large");
        }

        ParsedAlbumIdToEntry = AlbumIdToIsChosen
            .Where(kvp => Guid.TryParse(kvp.Key, out _))
            .ToDictionary(kvp => new VokiAlbumId(new(kvp.Key)), kvp => kvp.Value);
        if (ParsedAlbumIdToEntry.Count != AlbumIdToIsChosen.Count) {
            return ErrFactory.IncorrectFormat("Some of provided Album ids are incorrect");
        }

        return ErrOrNothing.Nothing;
    }

    public Dictionary<VokiAlbumId, bool> ParsedAlbumIdToEntry { get; private set; }
}