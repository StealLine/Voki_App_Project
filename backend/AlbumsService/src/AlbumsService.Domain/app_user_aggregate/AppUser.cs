using AlbumsService.Domain.app_user_aggregate.events;

namespace AlbumsService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiAlbumId> AlbumIds { get; private set; }
    public UserAutoAlbumsAppearance AutoAlbumsAppearance { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        AlbumIds = [];
        AutoAlbumsAppearance = UserAutoAlbumsAppearance.Default();
    }

    public const int MaxAlbumsForUser = 250;

    public ErrOrNothing AddAlbum(VokiAlbumId id) {
        if (AlbumIds.Count >= MaxAlbumsForUser) {
            return ErrFactory.LimitExceeded(
                $"User cannot have more than {MaxAlbumsForUser} albums. You already have {AlbumIds.Count}"
            );
        }

        AlbumIds = AlbumIds.Add(id);
        return ErrOrNothing.Nothing;
    }

    public void DeleteAlbum(VokiAlbumId id) {
        if (AlbumIds.Contains(id)) {
            AlbumIds = AlbumIds.Remove(id);
            AddDomainEvent(new VokiAlbumDeletedDomainEvent(id));
        }
    }

    public void UpdateAutoAlbumsAppearance(UserAutoAlbumsAppearance appearance) {
        AutoAlbumsAppearance = appearance;
    }
}