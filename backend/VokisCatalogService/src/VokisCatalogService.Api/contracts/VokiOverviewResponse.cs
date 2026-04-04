using System.Text.Json.Serialization;
using SharedKernel.common.vokis;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts;

public record class VokiOverviewResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] ManagerIds,
    Language Language,
    string Description,
    string[] Tags,
    bool HasMatureContent,
    DateTime PublicationDate,
    uint RatingsCount,
    uint CommentsCount,
    bool SignedInOnlyTaking,
    VokiOverviewResponse.VokiTypeWithSpecificDataResponse TypeSpecificData
) : ICreatableResponse<Voki>
{
    public static ICreatableResponse<Voki> Create(Voki voki) => FromBaseVoki(voki);

    public static VokiOverviewResponse FromBaseVoki(Voki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.Cover.ToString(),
        PrimaryAuthorId: v.PrimaryAuthorId.ToString(),
        CoAuthorIds: v.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        ManagerIds: v.ManagersSet.ToArray().Select(id => id.ToString()).ToArray(),
        v.Details.Language,
        v.Details.Description,
        v.Tags.Select(t => t.ToString()).ToArray(),
        v.Details.HasMatureContent,
        v.PublicationDate,
        v.RatingsCount,
        v.CommentsCount,
        v.InteractionSettings.SignedInOnlyTaking,
        CreateTypeSpecificData(v.TypeSpecificData)
    );

    private static VokiTypeWithSpecificDataResponse CreateTypeSpecificData(
        BaseVokiTypeSpecificData v
    ) => v.Match<VokiTypeWithSpecificDataResponse>(
        (g) => new GeneralVokiTypeWithSpecificDataResponse(
            ForceSequentialAnswering: false,
            ShuffleQuestions: false,
            AnyAudios: g.AnyAudios
        ),
        (t) => new TierListVokiTypeWithSpecificDataResponse(),
        (s) => new ScoringVokiTypeWithSpecificDataResponse()
    );

    [JsonDerivedType(typeof(GeneralVokiTypeWithSpecificDataResponse), nameof(GeneralVokiTypeWithSpecificDataResponse))]
    [JsonDerivedType(typeof(TierListVokiTypeWithSpecificDataResponse), nameof(TierListVokiTypeWithSpecificDataResponse))]
    [JsonDerivedType(typeof(ScoringVokiTypeWithSpecificDataResponse), nameof(ScoringVokiTypeWithSpecificDataResponse))]
    public abstract record VokiTypeWithSpecificDataResponse;


    public record GeneralVokiTypeWithSpecificDataResponse(
        bool ForceSequentialAnswering,
        bool ShuffleQuestions,
        bool AnyAudios
    ) : VokiTypeWithSpecificDataResponse();

    public record TierListVokiTypeWithSpecificDataResponse() : VokiTypeWithSpecificDataResponse();

    public record ScoringVokiTypeWithSpecificDataResponse() : VokiTypeWithSpecificDataResponse();
}