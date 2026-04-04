using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.contracts.managers;

public record VokiExpectedManagersSettingResponse(
    bool MakeAllCoAuthorsManagers,
    string[] UserIdsToBecomeManagers
) : ICreatableResponse<VokiExpectedManagersSetting>
{
    public static VokiExpectedManagersSettingResponse FromSetting(VokiExpectedManagersSetting setting) => new(
        setting.MakeAllCoAuthorsManagers,
        setting.UserIdsToBecomeManagers.ToArray().Select(t => t.ToString()).ToArray()
    );

    public static ICreatableResponse<VokiExpectedManagersSetting> Create(VokiExpectedManagersSetting s) => FromSetting(s);
}