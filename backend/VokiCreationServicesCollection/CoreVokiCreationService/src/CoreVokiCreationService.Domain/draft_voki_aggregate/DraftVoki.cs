using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.common.rules;
using SharedKernel.common.vokis;
using SharedKernel.user_ctx;
using VokimiStorageKeysLib.concrete_keys;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class DraftVoki : AggregateRoot<VokiId>
{
    private DraftVoki() { }
    public VokiType Type { get; }
    public VokiName Name { get; private set; }
    public VokiCoverKey Cover { get; private set; }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; private set; }
    public VokiExpectedManagersSetting ExpectedManagers { get; private set; }
    public ImmutableHashSet<AppUserId> InvitedForCoAuthorUserIds { get; private set; }
    public DateTime CreationDate { get; }

    private DraftVoki(
        VokiId id, VokiType type, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, DateTime creationDate
    ) {
        Id = id;
        Type = type;
        Name = name;
        Cover = cover;
        PrimaryAuthorId = primaryAuthorId;
        CreationDate = creationDate;
        CoAuthorIds = [];
        InvitedForCoAuthorUserIds = [];
        ExpectedManagers = VokiExpectedManagersSetting.AllCoAuthorsWillBecomeManagers();
    }

    public static DraftVoki Create(VokiName name, VokiType type, AuthenticatedUserCtx aUserCtx, DateTime creationDate) {
        VokiId vokiId = VokiId.CreateNew();
        VokiCoverKey cover = VokiCoverKey.CreateWithId(vokiId, CommonStorageItemKey.DefaultVokiCover.ImageExtension);
        DraftVoki newVoki = new(vokiId, type, name, cover, aUserCtx.UserId, creationDate);
        newVoki.AddDomainEvent(new NewDraftVokiInitializedEvent(
            newVoki.Id,
            newVoki.Type,
            newVoki.Name,
            newVoki.Cover,
            newVoki.PrimaryAuthorId,
            newVoki.CreationDate
        ));
        return newVoki;
    }


    public bool DoesUserHaveAccess(AuthenticatedUserCtx aUserCtx) =>
        aUserCtx.UserId == PrimaryAuthorId || CoAuthorIds.Contains(aUserCtx.UserId);

    public ErrOrNothing UpdateCover(VokiCoverKey newCover) {
        if (!newCover.IsWithId(this.Id)) {
            return ErrFactory.Conflict(
                "This cover does not belong to this Voki", $"Voki id: {Id}, cover voki id: {newCover.VokiId}"
            );
        }

        this.Cover = newCover;
        return ErrOrNothing.Nothing;
    }

    public void UpdateName(VokiName newName) {
        Name = newName;
    }

    public ErrOrNothing InviteCoAuthors(AuthenticatedUserCtx authenticatedUserCtx, ImmutableHashSet<AppUserId> userIdsToInvite) {
        if (authenticatedUserCtx.UserId != PrimaryAuthorId) {
            return ErrFactory.NoAccess("To invite co-authors you need to be the Voki primary author");
        }

        if (userIdsToInvite.Count == 0) {
            return ErrOrNothing.Nothing;
        }

        if (userIdsToInvite.Contains(PrimaryAuthorId)) {
            return ErrFactory.Conflict("You cannot invite the primary author of this Voki to be a co-author");
        }

        ImmutableHashSet<AppUserId> newInvitedNotAlreadyCoAuthors = userIdsToInvite.Except(CoAuthorIds);

        if (newInvitedNotAlreadyCoAuthors.Count == 0) {
            return ErrFactory.Conflict("All selected users are already co-authors");
        }

        ImmutableHashSet<AppUserId> uniqueNewInvites = newInvitedNotAlreadyCoAuthors.Except(InvitedForCoAuthorUserIds);

        if (uniqueNewInvites.Count == 0) {
            return ErrOrNothing.Nothing;
        }

        int totalAfterInvites = CoAuthorIds.Count + InvitedForCoAuthorUserIds.Count + uniqueNewInvites.Count;
        if (totalAfterInvites > VokiRules.MaxCoAuthors) {
            return ErrFactory.LimitExceeded(
                $"Voki cannot have more than {VokiRules.MaxCoAuthors} co-authors (including invited)"
            );
        }

        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Union(uniqueNewInvites);
        AddDomainEvent(new NewUsersInvitedForCoAuthorEvent(uniqueNewInvites, this.Id));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing AcceptInvite(AuthenticatedUserCtx userContext) {
        if (CoAuthorIds.Contains(userContext.UserId)) {
            return ErrOrNothing.Nothing;
        }

        if (PrimaryAuthorId == userContext.UserId) {
            return ErrFactory.Conflict("Primary author cannot become a co-author");
        }

        if (!InvitedForCoAuthorUserIds.Contains(userContext.UserId)) {
            return ErrFactory.Unspecified("You are not listed as invited in this voki");
        }

        if (CoAuthorIds.Count >= VokiRules.MaxCoAuthors) {
            return ErrFactory.Conflict(
                $"Some error has occured. This voki already has {VokiRules.MaxCoAuthors} co-authors"
            );
        }

        CoAuthorIds = CoAuthorIds.Add(userContext.UserId);
        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Remove(userContext.UserId);
        AddDomainEvent(new CoAuthorInviteAcceptedEvent(Id, userContext.UserId, Type, DecideUserIdsToBecomeManagers()));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing CancelCoAuthorInvite(AuthenticatedUserCtx aUserCtx, AppUserId userId) {
        if (aUserCtx.UserId != PrimaryAuthorId) {
            return ErrFactory.NoAccess("To cancel invites to become co-author you need to be the Voki primary author");
        }

        if (InvitedForCoAuthorUserIds.Contains(userId)) {
            InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Remove(userId);
            AddDomainEvent(new CoAuthorInviteCanceledEvent(Id, userId));
        }

        return ErrOrNothing.Nothing;
    }

    public void DeclineCoAuthorInvite(AppUserId userId) {
        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Remove(userId);
    }

    public void MarkAsPublished() {
        foreach (var invitedUserIds in InvitedForCoAuthorUserIds) {
            AddDomainEvent(new CoAuthorInviteCanceledEvent(Id, invitedUserIds));
        }

        AddDomainEvent(new VokiPublishedEvent(Id, PrimaryAuthorId, CoAuthorIds));
    }

    public ErrOrNothing DropCoAuthor(AuthenticatedUserCtx aUserCtx, AppUserId coAuthorId) {
        if (aUserCtx.UserId != PrimaryAuthorId) {
            return ErrFactory.NoAccess("To drop co-authors you need to be the Voki primary author");
        }

        if (CoAuthorIds.Contains(coAuthorId)) {
            CoAuthorIds = CoAuthorIds.Remove(coAuthorId);
            ExpectedManagers = ExpectedManagers.Without(coAuthorId);
            AddDomainEvent(new VokiCoAuthorRemovedEvent(Id, coAuthorId, this.Type, DecideUserIdsToBecomeManagers()));
        }

        return ErrOrNothing.Nothing;
    }

    public void LeaveVokiCreation(AuthenticatedUserCtx userContext) {
        if (!CoAuthorIds.Contains(userContext.UserId)) {
            return;
        }

        CoAuthorIds = CoAuthorIds.Remove(userContext.UserId);
        ExpectedManagers = ExpectedManagers.Without(userContext.UserId);
        AddDomainEvent(new VokiCoAuthorRemovedEvent(
            Id, userContext.UserId, this.Type,
            DecideUserIdsToBecomeManagers()
        ));
    }

    private ImmutableHashSet<AppUserId> DecideUserIdsToBecomeManagers() =>
        ExpectedManagers.DecideManagers(this.CoAuthorIds);

    public ErrOrNothing UpdateExpectedManagers(AuthenticatedUserCtx aUserCtx, VokiExpectedManagersSetting newExpectedManagers) {
        if (aUserCtx.UserId != PrimaryAuthorId) {
            return ErrFactory.NoAccess("To update Voki expected managers you need to be the Voki primary author");
        }

        if (newExpectedManagers.AnySpecifiedUser(u => !CoAuthorIds.Contains(u))) {
            return ErrFactory.Conflict("Some of users specified to become managers are not co-authors");
        }

        if (newExpectedManagers.SpecifiedUserContains(this.PrimaryAuthorId)) {
            return ErrFactory.Conflict("Primary author cannot be specified to become manager");
        }

        this.ExpectedManagers = newExpectedManagers;
        AddDomainEvent(new DraftVokiExpectedManagersUpdatedEvent(Id, this.Type, DecideUserIdsToBecomeManagers()));
        return ErrOrNothing.Nothing;
    }
}