using UserProfilesService.Application.app_users.queries;

namespace UserProfilesService.Api.contracts;

public record UserBasicProfileSetupInfoResponse(
    string UniqueName,
    string DisplayName,
    Language[] PreferredLanguages,
    string[] FavoriteTags,
    string ProfilePicKey,
    int MaxDisplayNameLength,
    int MaxTagLength
) : ICreatableResponse<GetCurrentUserBasicSetupInfoQueryResult>
{
    public static ICreatableResponse<GetCurrentUserBasicSetupInfoQueryResult> Create(
        GetCurrentUserBasicSetupInfoQueryResult user
    ) => new UserBasicProfileSetupInfoResponse(
        user.UniqueName.ToString(),
        user.DisplayName.ToString(),
        user.KnownLanguages.ToArray(),
        user.FavoriteTags.Select(t => t.ToString()).ToArray(),
        user.ProfilePicKey.ToString(),
        UserDisplayName.MaxLength,
        VokiTagId.MaxTagLength
    );
}