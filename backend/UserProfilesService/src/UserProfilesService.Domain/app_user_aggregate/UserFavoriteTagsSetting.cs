using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate;

public class UserFavoriteTagsSetting : ValueObject
{
    private UserFavoriteTagsSetting() { }
    private const int MaxTagsCount = 20;
    public ImmutableHashSet<VokiTagId> Tags { get; }
    public bool ShowOnProfile { get; }

    private UserFavoriteTagsSetting(ImmutableHashSet<VokiTagId> tags, bool showOnProfile) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(tags));
        Tags = tags;
        ShowOnProfile = showOnProfile;
    }

    public static UserFavoriteTagsSetting Default() => new([], false);

    public static ErrOrNothing CheckForErr(ISet<VokiTagId> tags) => tags.Count > MaxTagsCount
        ? ErrFactory.LimitExceeded($"Too many favourite tags chosen. Maximum count: {MaxTagsCount}")
        : ErrOrNothing.Nothing;

    public static ErrOr<UserFavoriteTagsSetting> Create(ImmutableHashSet<VokiTagId> tags, bool showOnProfile) =>
        CheckForErr(tags).IsErr(out var err) ? err : new UserFavoriteTagsSetting(tags, showOnProfile);

    public override IEnumerable<object> GetEqualityComponents() => [Tags, ShowOnProfile];
}