using SharedKernel.common.app_users;
using SharedKernel.exceptions;
using UserProfilesService.Domain.app_user_aggregate.dtos;
using UserProfilesService.Domain.app_user_aggregate.events;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public UserUniqueName UniqueName { get; private set; }
    public UserDisplayName DisplayName { get; private set; }
    public UserProfilePic ProfilePic { get; private set; }
    public UserLanguageSettings LanguageSettings { get; private set; }
    public UserFavoriteTagsSetting FavoriteTagsSetting { get; private set; }
    public UserFeaturedAuthorsSetting FeaturedAuthorsSetting { get; private set; }
    public UserFrontendSettings FrontendSettings { get; private set; }
    public UserProfileSettings ProfileSettings { get; private set; }
    public UserSocialInteractionSettings SocialInteractionSettings { get; private set; }

    public AppUser(AppUserId userId, UserUniqueName uniqueName, UserProfilePic profilePic) {
        if (!profilePic.Key.IsForUser(userId)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Conflict(
                $"Given profile pic key doesn't belong to this user. User id: {userId}, profile pic id: {profilePic.Key.UserId}"
            ));
        }

        Id = userId;
        UniqueName = uniqueName;
        DisplayName = UserDisplayName.FromUniqueName(uniqueName);
        ProfilePic = profilePic;

        FavoriteTagsSetting = UserFavoriteTagsSetting.Default();
        FeaturedAuthorsSetting = UserFeaturedAuthorsSetting.Default();
        FrontendSettings = UserFrontendSettings.Default();
        LanguageSettings = UserLanguageSettings.Default();
        ProfileSettings = UserProfileSettings.Default();
        SocialInteractionSettings = UserSocialInteractionSettings.Default;
    }

    public AppUser(AppUserId userId, UserUniqueName uniqueName, UserProfilePicKey profilePicKey)
        : this(userId, uniqueName, new UserProfilePic(profilePicKey, ProfilePicShape.Circle)) { }

    private ErrOrNothing CheckIfProfilePicIsForUser(UserProfilePicKey profilePic) => profilePic.IsForUser(Id)
        ? ErrOrNothing.Nothing
        : ErrFactory.Conflict(
            "Given profile pic key doesn't belong to this user",
            $" User id: {Id}, profile pic id: {profilePic.UserId}"
        );

    public ErrOrNothing UpdateProfilePic(UserProfilePic newUserProfilePic) {
        if (CheckIfProfilePicIsForUser(newUserProfilePic.Key).IsErr(out var err)) {
            return err;
        }

        ProfilePic = newUserProfilePic;

        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing ProcessBasicSetup(
        UserProfilePicKey profilePic,
        UserDisplayName displayName,
        UserLanguageSettings languageSettings,
        UserFavoriteTagsSetting favoriteTags
    ) {
        if (
            CheckIfProfilePicIsForUser(profilePic)
            .IsErr(out var err)
        ) {
            return err;
        }

        DisplayName = displayName;
        LanguageSettings = languageSettings;
        FavoriteTagsSetting = favoriteTags;
        ProfilePic = ProfilePic with {
            Key = profilePic
        };
        return ErrOrNothing.Nothing;
    }

    public UserProfileViewDto GetProfileViewData() => new(
        Banner: ProfileSettings.Banner,
        DisplayName: DisplayName,
        UniqueName: UniqueName,
        ProfilePic: ProfilePic,
        KnownLanguages: new UserProfilePossiblyHiddenField<IReadOnlyList<Language>>(
            Value: LanguageSettings.KnownLanguages.ToArray(),
            ShowOnProfile: ProfileSettings.Pronouns.ShowOnProfile
        ),
        Pronouns: new UserProfilePossiblyHiddenField<string>(
            Value: ProfileSettings.Pronouns.Value,
            ShowOnProfile: ProfileSettings.Pronouns.ShowOnProfile
        ),
        Status: new UserProfilePossiblyHiddenField<string>(
            Value: ProfileSettings.Status.Value,
            ShowOnProfile: ProfileSettings.Status.ShowOnProfile
        ),
        AboutMe: new UserProfilePossiblyHiddenField<string>(
            Value: ProfileSettings.AboutMe.Value,
            ShowOnProfile: ProfileSettings.AboutMe.ShowOnProfile
        ),
        Links: new UserProfilePossiblyHiddenField<IReadOnlyList<UserLink>>(
            Value: ProfileSettings.Links.Links.ToArray(),
            ShowOnProfile: ProfileSettings.Links.ShowOnProfile
        ),
        FavoriteTags: new UserProfilePossiblyHiddenField<IReadOnlyList<string>>(
            Value: FavoriteTagsSetting.Tags
                .Select(x => x.ToString())
                .ToArray(),
            ShowOnProfile: FavoriteTagsSetting.ShowOnProfile
        ),
        FavoriteAuthorIds: new UserProfilePossiblyHiddenField<IReadOnlyList<AppUserId>>(
            Value: FeaturedAuthorsSetting.UserIds.ToArray(),
            ShowOnProfile: FeaturedAuthorsSetting.ShowOnProfile
        )
    );
}