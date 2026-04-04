using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts.managers;

public class UpdateVokiExpectedManagersRequest : IRequestWithValidationNeeded
{
    public bool MakeAllCoAuthorsManagers { get; init; }
    public string[] UserIdsToBecomeManagers { get; init; }

    public ErrOrNothing Validate() {
        if (MakeAllCoAuthorsManagers) {
            ParsedSetting = VokiExpectedManagersSetting.AllCoAuthorsWillBecomeManagers();
            return ErrOrNothing.Nothing;
        }

        var parsedIds = UserIdsToBecomeManagers
            .Where(u => Guid.TryParse(u, out _))
            .Select(u => new AppUserId(new(u)))
            .ToImmutableHashSet();

        if (parsedIds.Count < UserIdsToBecomeManagers.Length) {
            return ErrFactory.IncorrectFormat("Some of the provided userIds are invalid");
        }

        var setRes = VokiManagersIdsSet.Create(parsedIds);
        if (setRes.IsErr(out var err)) {
            return err;
        }

        var settingsRes = VokiExpectedManagersSetting.SpecifiedUsers(setRes.AsSuccess());
        if (settingsRes.IsErr(out err)) {
            return err;
        }

        ParsedSetting = settingsRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public VokiExpectedManagersSetting ParsedSetting { get; private set; }
}