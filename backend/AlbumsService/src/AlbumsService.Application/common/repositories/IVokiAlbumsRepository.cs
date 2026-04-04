using AlbumsService.Domain.voki_album_aggregate;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.common.repositories;

public interface IVokiAlbumsRepository
{
    Task<VokiAlbum[]> ListAlbumsForUser(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task<VokiAlbum[]> ListUsersAlbumsForUpdate(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task<VokiAlbumPreviewDto[]> GetCurrentUserAlbumPreviewsSorted(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task Add(VokiAlbum album, CancellationToken ct);
    Task<VokiAlbum?> GetByIdForUpdate(VokiAlbumId albumId, CancellationToken ct);
    Task DeleteAlbum(VokiAlbum album, CancellationToken ct);
    Task UpdateRange(IEnumerable<VokiAlbum> albums, CancellationToken ct);
    Task<VokiAlbum?> GetById(VokiAlbumId albumId, CancellationToken ct);
    Task Update(VokiAlbum album, CancellationToken ct);
    Task<VokiAlbum[]> ListByIds(IEnumerable<VokiAlbumId> ids, CancellationToken ct);
}

public record VokiAlbumPreviewDto(
    VokiAlbumId Id,
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor,
    int VokiIdsCount,
    DateTime CreationDate
);