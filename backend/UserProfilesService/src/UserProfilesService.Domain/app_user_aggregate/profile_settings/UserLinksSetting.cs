using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate.profile_settings;

public class UserLinksSetting : ValueObject
{
    private UserLinksSetting() { }
    public const int MaxLinksCount = 15;

    private UserLinksSetting(bool showOnProfile, ImmutableArray<UserLink> links) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(showOnProfile, links));
        ShowOnProfile = showOnProfile;
        Links = links;
    }

    public bool ShowOnProfile { get; }
    public ImmutableArray<UserLink> Links { get; }

    public static UserLinksSetting Default() => new(
        false,
        ImmutableArray<UserLink>.Empty
    );

    public static ErrOr<UserLinksSetting> Create(bool showOnProfile, ImmutableArray<UserLink> links) =>
        CheckForErr(showOnProfile, links).IsErr(out var err) ? err : new UserLinksSetting(showOnProfile, links);

    public static ErrOrNothing CheckForErr(bool showOnProfile, ImmutableArray<UserLink> links) {
        if (links.Length > MaxLinksCount) {
            return ErrFactory.LimitExceeded(
                $"Too many links selected. User cannot specify more than {MaxLinksCount} in their profile"
            );
        }

        var uniqueLinksCount = links.Select(l => l.Value).ToHashSet().Count;
        if (uniqueLinksCount != links.Length) {
            return ErrFactory.Conflict(
                $"Some of the provided links are the same. Unique links count: {uniqueLinksCount}. Provided links count: {links.Length}",
                "Please remove duplicate links"
            );
        }

        if (showOnProfile && links.Length == 0) {
            return ErrFactory.Conflict("No links to show in profile. Please add at least one link");
        }

        return ErrOrNothing.Nothing;
    }


    public override IEnumerable<object> GetEqualityComponents() => [
        ShowOnProfile,
        Links.Select(l => (l.Value, l.Type))
    ];
}

public class UserLink
{
    public string Value { get; }
    public UserLinkType Type { get; }

    private UserLink(string value, UserLinkType type) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value, type));
        Value = value;
        Type = type;
    }

    private static ErrOrNothing CheckForErr(string value, UserLinkType type) {
        if (string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.NoValue.Common("Link cannot be empty");
        }

        bool startsWithHttp = value.StartsWith("https://") || value.StartsWith("http://");
        if (!startsWithHttp) {
            return ErrFactory.NoValue.Common("Incorrect link format. Link must start with https:// or http://");
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOr<UserLink> Create(string value, UserLinkType type) =>
        CheckForErr(value, type).IsErr(out var err) ? err : new UserLink(value, type);
}

public enum UserLinkType
{
    Other = 0,
    Website = 1,
    X = 2,
    Instagram = 3,
    YouTube = 4,
    TikTok = 6,
    Telegram = 7,
    Mangalib = 8,
    Reddit = 9
}