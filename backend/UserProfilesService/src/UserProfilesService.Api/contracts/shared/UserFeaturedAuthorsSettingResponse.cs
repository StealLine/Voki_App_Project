namespace UserProfilesService.Api.contracts.shared;

public record UserFeaturedAuthorsSettingResponse(bool ShowOnProfile, string[] AuthorIds)
{
    public static UserFeaturedAuthorsSettingResponse Create(UserFeaturedAuthorsSetting s) => new(
        s.ShowOnProfile,
        s.UserIds.Select(id => id.ToString()).ToArray()
    );
}