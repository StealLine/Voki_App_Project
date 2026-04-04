using System.Collections.Immutable;
using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace AlbumsService.Api.contracts.copy_vokis_from_albums_to_album;

public class CopyVokisFromAlbumsToAlbumRequest : IRequestWithValidationNeeded
{
    public string[] AlbumIds { get; init; }

    public ErrOrNothing Validate() {
        if (AlbumIds is null || AlbumIds.Length == 0) {
            return ErrFactory.NoValue.Common("No albums to copy from provided");
        }

        if (AlbumIds.Length > VokiAlbum.MaxAlbumsToCopyFrom) {
            return ErrFactory.LimitExceeded(
                $"Too many source albums specified. Maximum allowed: {AlbumIds}",
                $"Provided: {AlbumIds.Length}"
            );
        }

        ParsedAlbumIds = AlbumIds
            .Where(a => Guid.TryParse(a, out _))
            .Select(a => new VokiAlbumId(new(a)))
            .ToImmutableHashSet();
        if (ParsedAlbumIds.Count == 0) {
            return ErrFactory.NoValue.Common("All provided album ids are incorrect");
        }

        return ErrOrNothing.Nothing;
    }


    public ImmutableHashSet<VokiAlbumId> ParsedAlbumIds { get; private set; }
}