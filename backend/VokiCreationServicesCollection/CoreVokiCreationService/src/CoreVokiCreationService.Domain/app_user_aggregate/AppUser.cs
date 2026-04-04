using CoreVokiCreationService.Domain.app_user_aggregate.events;
using SharedKernel.common.app_users;

namespace CoreVokiCreationService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiId> InitializedVokiIds { get; private set; }
    public ImmutableHashSet<VokiId> CoAuthoredVokiIds { get; private set; }
    public ImmutableHashSet<VokiId> InvitedToCoAuthorVokiIds { get; private set; }
    public AllowCoAuthorInvitesSettingValue InvitesSetting { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        InitializedVokiIds = [];
        CoAuthoredVokiIds = [];
        InvitedToCoAuthorVokiIds = [];
        InvitesSetting = AllowCoAuthorInvitesSettingValueExtensions.Default;
    }

   
    public void AddInitializedVoki(VokiId vokiId) {
        InitializedVokiIds = InitializedVokiIds.Add(vokiId);
    }

    public ErrOrNothing InviteForCoAuthor(VokiId vokiId) {
        if (InitializedVokiIds.Contains(vokiId)) {
            return ErrFactory.Conflict("User cannot be invite to co-author in voki that they have initialized");
        }

        if (CoAuthoredVokiIds.Contains(vokiId)) {
            return ErrFactory.Conflict("User is already listed as voki co-author");
        }

        InvitedToCoAuthorVokiIds = InvitedToCoAuthorVokiIds.Add(vokiId);
        return ErrOrNothing.Nothing;
    }

    public void DeclineCoAuthorInvite(VokiId vokiId) {
        if (!InvitedToCoAuthorVokiIds.Contains(vokiId)) {
            return;
        }

        InvitedToCoAuthorVokiIds = InvitedToCoAuthorVokiIds.Remove(vokiId);
        AddDomainEvent(new CoAuthorInviteDeclinedEvent(vokiId, Id));
    }

    public ErrOrNothing AcceptCoAuthorInvite(VokiId vokiId) {
        if (CoAuthoredVokiIds.Contains(vokiId)) {
            InvitedToCoAuthorVokiIds = InvitedToCoAuthorVokiIds.Remove(vokiId);
            return ErrOrNothing.Nothing;
        }

        if (!InvitedToCoAuthorVokiIds.Contains(vokiId)) {
            return ErrFactory.Unspecified("You are not listed as invited in this voki");
        }

        CoAuthoredVokiIds = CoAuthoredVokiIds.Add(vokiId);
        InvitedToCoAuthorVokiIds = InvitedToCoAuthorVokiIds.Remove(vokiId);
        return ErrOrNothing.Nothing;
    }

    public void RemoveInvitedToCoAuthorVoki(VokiId vokiId) {
        InvitedToCoAuthorVokiIds = InvitedToCoAuthorVokiIds.Remove(vokiId);
    }

    public void RemoveInitializedVoki(VokiId vokiId) =>
        InitializedVokiIds = InitializedVokiIds.Remove(vokiId);

    public void RemoveCoAuthoredVoki(VokiId vokiId) =>
        CoAuthoredVokiIds = CoAuthoredVokiIds.Remove(vokiId);
}