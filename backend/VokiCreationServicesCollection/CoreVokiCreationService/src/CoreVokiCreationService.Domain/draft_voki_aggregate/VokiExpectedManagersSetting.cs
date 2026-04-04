using System.Diagnostics.Contracts;
using SharedKernel.common.rules;
using SharedKernel.common.vokis;
using SharedKernel.exceptions;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class VokiExpectedManagersSetting : ValueObject
{
    public bool MakeAllCoAuthorsManagers { get; }
    public VokiManagersIdsSet UserIdsToBecomeManagers { get; }
    public override IEnumerable<object> GetEqualityComponents() => [MakeAllCoAuthorsManagers, UserIdsToBecomeManagers];

    private VokiExpectedManagersSetting(
        bool makeAllCoAuthorsManagers,
        VokiManagersIdsSet userIdsToBecomeManagers
    ) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(userIdsToBecomeManagers));
        MakeAllCoAuthorsManagers = makeAllCoAuthorsManagers;
        UserIdsToBecomeManagers = userIdsToBecomeManagers;
    }

    private static ErrOrNothing CheckForErr(VokiManagersIdsSet userIdsToBecomeManagers) =>
        userIdsToBecomeManagers.Count() > VokiRules.MaxCoAuthors
            ? ErrFactory.Conflict("Cannot specify more users to become managers than Voki can have as co-authors")
            : ErrOrNothing.Nothing;

    public static VokiExpectedManagersSetting AllCoAuthorsWillBecomeManagers() => new(true, VokiManagersIdsSet.Empty);

    public static ErrOr<VokiExpectedManagersSetting> SpecifiedUsers(VokiManagersIdsSet userIdsToBecomeManagers) =>
        CheckForErr(userIdsToBecomeManagers).IsErr(out var err)
            ? err
            : new VokiExpectedManagersSetting(false, userIdsToBecomeManagers);

    [Pure]
    public VokiExpectedManagersSetting Without(AppUserId userId) => new(
        this.MakeAllCoAuthorsManagers,
        this.UserIdsToBecomeManagers.Remove(userId)
    );

    [Pure]
    public ImmutableHashSet<AppUserId> DecideManagers(ImmutableHashSet<AppUserId> coAuthors) =>
        MakeAllCoAuthorsManagers ? coAuthors.ToImmutableHashSet() : UserIdsToBecomeManagers.ToImmutableHashSet();

    [Pure]
    public bool AnySpecifiedUser(Func<AppUserId, bool> predicate) => UserIdsToBecomeManagers.Any(predicate);

    [Pure]
    public bool SpecifiedUserContains(AppUserId userId) => UserIdsToBecomeManagers.Contains(userId);
}