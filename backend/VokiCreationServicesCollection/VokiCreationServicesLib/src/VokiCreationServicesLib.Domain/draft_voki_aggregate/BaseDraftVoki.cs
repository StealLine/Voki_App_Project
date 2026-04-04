using System.Collections.Immutable;
using SharedKernel;
using SharedKernel.common.vokis;
using SharedKernel.user_ctx;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;
using VokimiStorageKeysLib.concrete_keys;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate;

public abstract class BaseDraftVoki : AggregateRoot<VokiId>
{
    protected BaseDraftVoki() { }
    public AppUserId PrimaryAuthorId { get; }
    public VokiCoAuthorIdsSet CoAuthors { get; private set; }
    public ImmutableHashSet<AppUserId> UserIdsToBecomeManagers { get; private set; }
    public VokiName Name { get; private set; }
    public VokiCoverKey Cover { get; private set; }
    public VokiDetails Details { get; private set; }
    public VokiTagsSet Tags { get; private set; }
    public DateTime CreationDate { get; }

    protected BaseDraftVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverKey cover,
        DateTime creationDate
    ) {
        Id = vokiId;
        PrimaryAuthorId = primaryAuthorId;
        CoAuthors = VokiCoAuthorIdsSet.Empty;
        UserIdsToBecomeManagers = [];

        Name = name;
        Cover = cover;
        Details = VokiDetails.Default;
        Tags = VokiTagsSet.Empty;
        CreationDate = creationDate;
    }

    public ErrOrNothing AddCoAuthor(AppUserId newCoAuthorId, ImmutableHashSet<AppUserId> newUserIdsToBecomeManagers) {
        if (newCoAuthorId == PrimaryAuthorId) {
            return ErrFactory.Conflict("Primary author cannot be a co-author of the Voki");
        }

        ErrOr<VokiCoAuthorIdsSet> newSetRes = CoAuthors.Add(newCoAuthorId);
        if (newSetRes.IsErr(out var err)) {
            return err;
        }

        CoAuthors = newSetRes.AsSuccess();
        UserIdsToBecomeManagers = NormalizeUsersToBecomeManagers(newUserIdsToBecomeManagers);
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing RemoveCoAuthor(AppUserId coAuthorId, ImmutableHashSet<AppUserId> newUserIdsToBecomeManagers) {
        if (coAuthorId == PrimaryAuthorId) {
            return ErrFactory.Conflict("Primary author cannot be removed from co-authors");
        }

        ErrOr<VokiCoAuthorIdsSet> newSetRes = CoAuthors.Remove(coAuthorId);
        if (newSetRes.IsErr(out var err)) {
            return err;
        }

        CoAuthors = newSetRes.AsSuccess();
        UserIdsToBecomeManagers = NormalizeUsersToBecomeManagers(newUserIdsToBecomeManagers);
        return ErrOrNothing.Nothing;
    }

    public void UpdateExpectedManagers(ImmutableHashSet<AppUserId> newUserIdsToBecomeManagers) {
        UserIdsToBecomeManagers = NormalizeUsersToBecomeManagers(newUserIdsToBecomeManagers);
    }

    private ImmutableHashSet<AppUserId> NormalizeUsersToBecomeManagers(ImmutableHashSet<AppUserId> candidateManagers) =>
        candidateManagers.Intersect(CoAuthors.ToImmutableHashSet());

    public bool HasUserAccess(AuthenticatedUserCtx aUserCtx) =>
        aUserCtx.UserId == PrimaryAuthorId || CoAuthors.Contains(aUserCtx.UserId);

    public static bool HasUserAccess(
        AuthenticatedUserCtx userContext,
        AppUserId primaryAuthorId,
        VokiCoAuthorIdsSet coAuthorIds
    ) {
        return userContext.UserId == primaryAuthorId || coAuthorIds.Contains(userContext.UserId);
    }

    public ErrOrNothing UpdateName(AuthenticatedUserCtx aUserCtx, VokiName newVokiName) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To update Voki name you need to be the Voki author");
        }

        Name = newVokiName;
        AddDomainEvent(new VokiNameUpdatedEvent(Id, Name));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateCover(AuthenticatedUserCtx aUserCtx, VokiCoverKey newCover) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To update Voki cover you need to be the Voki author");
        }

        if (!newCover.IsWithId(this.Id)) {
            return ErrFactory.Conflict(
                "This cover does not belong to this Voki", $"Voki id: {Id}, cover voki id: {newCover.VokiId}"
            );
        }

        VokiCoverKey oldCover = this.Cover;
        this.Cover = newCover;
        AddDomainEvent(new VokiCoverUpdatedEvent(Id, OldCover: oldCover, Cover));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateDetails(AuthenticatedUserCtx aUserCtx, VokiDetails newDetails) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To update Voki details you need to be the Voki author");
        }

        this.Details = newDetails;
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateTags(AuthenticatedUserCtx aUserCtx, VokiTagsSet newTags) {
        if (!HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("To update Voki tags you need to be the Voki author");
        }

        this.Tags = newTags;
        return ErrOrNothing.Nothing;
    }

    protected ErrOrNothing CheckConfirmedCoAuthorsAndManagers(
        ISet<AppUserId> confirmedCoAuthorIds, ISet<AppUserId> confirmedManagerIds
    ) {
        ErrOrNothing errs = ErrOrNothing.Nothing;
        if (!CoAuthors.SetEquals(confirmedCoAuthorIds)) {
            errs.AddNext(ErrFactory.Conflict(
                "Voki co-authors were changed and now differ from the confirmed. Please refresh the page and try again"
            ));
        }

        if (!UserIdsToBecomeManagers.SetEquals(confirmedManagerIds)) {
            errs.AddNext(ErrFactory.Conflict(
                "Users to become managers were changed and now differ from the confirmed. Please refresh the page and try again"
            ));
        }

        return errs;
    }
    protected List<VokiPublishingIssue> CheckCoverForPublishingIssues() {
        if (!Cover.IsWithId(Id)) {
            return [
                VokiPublishingIssue.Problem(
                    message: "Cover does not belong to this Voki",
                    source: "Cover",
                    fixRecommendation: "Try choosing another cover"
                )
            ];
        }

        return [];
    }

    protected List<VokiPublishingIssue> CheckDetailsForPublishingIssues() {
        if (string.IsNullOrWhiteSpace(Details.Description.ToString())) {
            return [
                VokiPublishingIssue.Warning(
                    message: "Description is empty",
                    source: "Description",
                    fixRecommendation: "Add a description to help users understand your Voki"
                )
            ];
        }

        return [];
    }

    protected List<VokiPublishingIssue> CheckTagsForPublishingIssues() {
        if (Tags.Value.Count == 0) {
            return [
                VokiPublishingIssue.Warning(
                    message: "No tags have been added",
                    source: "Tags",
                    fixRecommendation: "Add some tags so users can discover your Voki more easily"
                )
            ];
        }

        return [];
    }
}