namespace UserProfilesService.Api.contracts.shared;

public sealed record UserProfilePicResponse(string Key, ProfilePicShape Shape)
{
    public static UserProfilePicResponse Create(UserProfilePic v) => new(
        v.Key.ToString(), v.Shape
    );
}