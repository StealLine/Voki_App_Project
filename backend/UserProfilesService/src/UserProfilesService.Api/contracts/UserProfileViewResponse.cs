using System.Text.Json.Serialization;
using UserProfilesService.Api.contracts.shared;
using UserProfilesService.Application.dtos;
using UserProfilesService.Domain.app_user_aggregate.dtos;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Api.contracts;

public sealed record UserProfileViewResponse(
    IUserBannerPrimitiveDto Banner,
    string DisplayName,
    string UniqueName,
    UserProfilePicResponse ProfilePic,
    PossiblyHiddenProfileFieldResponse<Language[]> KnownLanguages,
    PossiblyHiddenProfileFieldResponse<string> Pronouns,
    PossiblyHiddenProfileFieldResponse<string> Status,
    PossiblyHiddenProfileFieldResponse<string> AboutMe,
    PossiblyHiddenProfileFieldResponse<UserProfileLinkResponse[]> Links,
    PossiblyHiddenProfileFieldResponse<string[]> FavouriteTags,
    PossiblyHiddenProfileFieldResponse<string[]> FavouriteAuthorIds
) : ICreatableResponse<UserProfileViewDto>
{
    public static ICreatableResponse<UserProfileViewDto> Create(UserProfileViewDto d) =>
        new UserProfileViewResponse(
            Banner: IUserBannerPrimitiveDto.Create(d.Banner),
            DisplayName: d.DisplayName.ToString(),
            UniqueName: d.UniqueName.ToString(),
            ProfilePic: UserProfilePicResponse.Create(d.ProfilePic),
            KnownLanguages: PossiblyHiddenProfileFieldResponse<Language[]>.Create(
                d.KnownLanguages,
                langs => langs.ToArray()
            ),
            Pronouns: PossiblyHiddenProfileFieldResponse<string>.Create(d.Pronouns),
            Status: PossiblyHiddenProfileFieldResponse<string>.Create(d.Status),
            AboutMe: PossiblyHiddenProfileFieldResponse<string>.Create(d.AboutMe),
            Links: PossiblyHiddenProfileFieldResponse<UserProfileLinkResponse[]>.Create(
                d.Links, links => links
                    .Select(l => new UserProfileLinkResponse(l.Value, l.Type))
                    .ToArray()
            ),
            FavouriteTags: PossiblyHiddenProfileFieldResponse<string[]>.Create(
                d.FavoriteTags,
                tags => tags.ToArray()
            ),
            FavouriteAuthorIds: PossiblyHiddenProfileFieldResponse<string[]>.Create(
                d.FavoriteAuthorIds,
                ids => ids.Select(x => x.ToString()).ToArray()
            )
        );
}

public sealed record UserProfileLinkResponse(string Value, UserLinkType Type);

public sealed record PossiblyHiddenProfileFieldResponse<T> where T : notnull
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Value { get; private init; }

    public bool ShowOnProfile { get; private init; }

    private static PossiblyHiddenProfileFieldResponse<T> Visible(T value) => new() {
        ShowOnProfile = true,
        Value = value
    };

    private static PossiblyHiddenProfileFieldResponse<T> Hidden() => new() {
        ShowOnProfile = false
    };

    public static PossiblyHiddenProfileFieldResponse<T> Create(UserProfilePossiblyHiddenField<T> source) =>
        source.ShowOnProfile
            ? Visible(source.Value)
            : Hidden();

    public static PossiblyHiddenProfileFieldResponse<T> Create<TSource>(
        UserProfilePossiblyHiddenField<TSource> source,
        Func<TSource, T> map
    ) => source.ShowOnProfile
        ? Visible(map(source.Value))
        : Hidden();
}