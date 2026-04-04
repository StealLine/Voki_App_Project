namespace UserProfilesService.Api.contracts.shared;

public record UserFavoriteTagsSettingResponse(bool ShowOnProfile, string[] Tags)
{
    public static UserFavoriteTagsSettingResponse Create(UserFavoriteTagsSetting s) => new(
        s.ShowOnProfile,
        s.Tags.Select(t => t.ToString()).ToArray()
    );
}
