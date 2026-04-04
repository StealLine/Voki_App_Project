using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Domain.app_user_aggregate;

public record UserProfilePic(
    UserProfilePicKey Key,
    ProfilePicShape Shape
);

public enum ProfilePicShape
{
    Circle,
    Squircle,
    RoundedSquare20,
    RoundedSquare30,
    Seal
}