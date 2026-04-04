using CoreVokiCreationService.Api.contracts.managers;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.rules;

namespace CoreVokiCreationService.Api.contracts;

internal record class VokiAuthorsInfoResponse(
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] InvitedForCoAuthorUserIds,
    DateTime VokiCreationDate,
    int MaxVokiCoAuthors,
    VokiExpectedManagersSettingResponse ExpectedManagers
) : ICreatableResponse<DraftVoki>
{
    public static ICreatableResponse<DraftVoki> Create(DraftVoki voki) => new VokiAuthorsInfoResponse(
        voki.PrimaryAuthorId.ToString(),
        voki.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        voki.InvitedForCoAuthorUserIds.Select(id => id.ToString()).ToArray(),
        voki.CreationDate,
        VokiRules.MaxCoAuthors,
        VokiExpectedManagersSettingResponse.FromSetting(voki.ExpectedManagers)
    );
}