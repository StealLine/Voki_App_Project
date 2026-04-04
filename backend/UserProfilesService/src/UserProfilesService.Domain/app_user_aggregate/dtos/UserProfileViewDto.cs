using SharedKernel.common.app_users;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Domain.app_user_aggregate.dtos;

public sealed record UserProfileViewDto(
    UserBanner Banner,
    UserDisplayName DisplayName,
    UserUniqueName UniqueName,
    UserProfilePic ProfilePic,
    UserProfilePossiblyHiddenField<IReadOnlyList<Language>> KnownLanguages,
    UserProfilePossiblyHiddenField<string> Pronouns,
    UserProfilePossiblyHiddenField<string> Status,
    UserProfilePossiblyHiddenField<string> AboutMe,
    UserProfilePossiblyHiddenField<IReadOnlyList<UserLink>> Links,
    UserProfilePossiblyHiddenField<IReadOnlyList<string>> FavoriteTags,
    UserProfilePossiblyHiddenField<IReadOnlyList<AppUserId>> FavoriteAuthorIds
);

public sealed record UserProfilePossiblyHiddenField<T>(
    T Value,
    bool ShowOnProfile
);