using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Domain.app_user_aggregate;

public sealed class UserProfileSettings : ValueObject
{
    private UserProfileSettings() { }

    public UserBanner Banner { get; }
    public UserStatus Status { get; }
    public UserPronouns Pronouns { get; }
    public UserAboutMe AboutMe { get; }
    public UserLinksSetting Links { get; }

    private UserProfileSettings(
        UserBanner banner,
        UserStatus status,
        UserPronouns pronouns,
        UserAboutMe aboutMe,
        UserLinksSetting links
    ) {
        Banner = banner;
        Status = status;
        Pronouns = pronouns;
        AboutMe = aboutMe;
        Links = links;
    }

    public static UserProfileSettings Create(
        UserBanner banner,
        UserStatus status,
        UserPronouns pronouns,
        UserAboutMe aboutMe,
        UserLinksSetting links
    ) => new(banner, status, pronouns, aboutMe, links);

    public static UserProfileSettings Default() => new(
        new UserBanner.Default(),
        UserStatus.Default(),
        UserPronouns.Default(),
        UserAboutMe.Default(),
        UserLinksSetting.Default()
    );

    public override IEnumerable<object> GetEqualityComponents() => [
        Banner,
        Status,
        Pronouns,
        AboutMe,
        Links
    ];
}