using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys.profile_pics;
using VokimiStorageKeysLib.temp_keys;

namespace UserProfilesService.Api.contracts;

public class SaveBasicProfileSetupRequest : IRequestWithValidationNeeded
{
    public string ProfilePic { get; init; }
    public string UserDisplayName { get; init; }
    public Language[] PreferredLanguages { get; init; }
    public string[] Tags { get; init; }

    const int MaxLangsLength = 1000;
    const int MaxTagsCount = 1000;

    public ErrOrNothing Validate() {
        if (PreferredLanguages.Length > MaxLangsLength) {
            return ErrFactory.LimitExceeded($"To many (more than {MaxLangsLength}) languages selected");
        }

        if (Tags.Length > 1000) {
            return ErrFactory.LimitExceeded($"To many (more than {MaxTagsCount}) tags selected");
        }

        ErrOrNothing errs = ErrOrNothing.Nothing;
        if (
            !UserProfilePicKey.IsStringWithPicsPrefix(ProfilePic)
            && !TempImageKey.IsPossiblySuitable(ProfilePic)
            && !PresetProfilePicKey.IsStringPresetKey(ProfilePic)
        ) {
            return ErrFactory.IncorrectFormat(
                "Incorrect profile picture format",
                $"Provided path should be either {nameof(UserProfilePicKey)}, {nameof(TempImageKey)} or {nameof(PresetProfilePicKey)}"
            );
        }

        var displayNameCreation = Domain.app_user_aggregate.UserDisplayName.Create(UserDisplayName);
        if (displayNameCreation.IsErr(out var err)) {
            return err;
        }

        ParsedDisplayName = displayNameCreation.AsSuccess();

        string[] incorrectTags = Tags.Where(t => !VokiTagId.IsStringValidTag(t)).ToArray();
        if (incorrectTags.Length > 0) {
            errs.AddNext(ErrFactory.IncorrectFormat(
                "Some of the selected tags are invalid",
                $"Incorrect tags: {string.Join(", ", incorrectTags)})"
            ));
        }

        if (errs.IsErr(out err)) {
            return err;
        }


        ParsedTags = Tags.Select(t => new VokiTagId(t)).ToImmutableHashSet();
        return ErrOrNothing.Nothing;
    }

    public ImmutableHashSet<VokiTagId> ParsedTags { get; private set; }
    public UserDisplayName ParsedDisplayName { get; private set; }
}