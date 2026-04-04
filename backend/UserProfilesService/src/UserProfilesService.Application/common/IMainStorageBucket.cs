using VokimiStorageKeysLib.concrete_keys.profile_pics;
using VokimiStorageKeysLib.temp_keys;

namespace UserProfilesService.Application.common;

public interface IMainStorageBucket
{
    Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromPresets(
        PresetProfilePicKey presetKey,
        AppUserId userId,
        CancellationToken ct
    );

    Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromTemp(
        TempImageKey temp,
        AppUserId userId,
        CancellationToken ct
    );
}