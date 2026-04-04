using ApiShared;
using VokiCommentsService.Application.common.repositories;

namespace VokiCommentsService.Api.contracts;

public record class UserCommentedVokiIdsResponse(
    Dictionary<string, DateTime> VokiIdWithCommentDate
) : ICreatableResponse<VokiIdWithLastCommentedDateDto[]>
{
    public static ICreatableResponse<VokiIdWithLastCommentedDateDto[]> Create(
        VokiIdWithLastCommentedDateDto[] vokis
    ) => new UserCommentedVokiIdsResponse(
        vokis.ToDictionary(v => v.VokiId.ToString(), v => v.Date)
    );
}