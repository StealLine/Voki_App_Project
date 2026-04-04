namespace AlbumsService.Domain.app_user_aggregate.events;

public record VokiAlbumDeletedDomainEvent(
    VokiAlbumId AlbumId
) : IDomainEvent;