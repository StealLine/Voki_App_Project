using SharedKernel.common.vokis.general_vokis;
using SharedKernel.common.vokis.scoring_voki;
using SharedKernel.common.vokis.tier_list_voki;
using VokimiStorageKeysLib.concrete_keys;
using VokisCatalogService.Domain.voki_aggregate.events;
using VokisCatalogService.Domain.voki_aggregate.type_specific_data;

namespace VokisCatalogService.Domain.voki_aggregate;

public class Voki : AggregateRoot<VokiId>
{
    protected Voki() { }

    public VokiType Type { get; }
    public VokiName Name { get; }
    public VokiCoverKey Cover { get; }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; }
    public VokiManagersIdsSet ManagersSet { get; private set; }
    public VokiDetails Details { get; }
    public ImmutableHashSet<VokiTagId> Tags { get; }
    public DateTime PublicationDate { get; }
    public uint RatingsCount { get; private set; }
    public uint CommentsCount { get; private set; }
    public uint VokiTakingsCount { get; private set; }
    public BaseVokiInteractionSettings InteractionSettings { get; private set; }
    public BaseVokiTypeSpecificData TypeSpecificData { get; }
    public CatalogPageSettings CatalogPageSettings { get; private set; }

    private Voki(
        VokiId id, VokiType type, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds, VokiManagersIdsSet managers,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publicationDate,
        BaseVokiInteractionSettings interactionSettings, BaseVokiTypeSpecificData typeSpecificData
    ) {
        Id = id;
        Type = type;
        Name = name;
        Cover = cover;
        PrimaryAuthorId = primaryAuthorId;
        CoAuthorIds = coAuthorIds;
        ManagersSet = managers;
        Details = details;
        Tags = tags;
        PublicationDate = publicationDate;
        RatingsCount = 0;
        CommentsCount = 0;
        VokiTakingsCount = 0;
        InteractionSettings = interactionSettings;
        TypeSpecificData = typeSpecificData;
        CatalogPageSettings = CatalogPageSettings.Default;

        AddDomainEvent(new PublishedVokiCreatedEvent(
            Id, PrimaryAuthorId, CoAuthorIds, Tags)
        );
    }

    public static Voki CreateGeneral(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds, VokiManagersIdsSet managers,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publishedDate,
        GeneralVokiInteractionSettings interactionSettings, GeneralVokiTypeSpecificData typeSpecificData
    ) => new(
        id, VokiType.General, name, cover, primaryAuthorId, coAuthorIds, managers, details, tags, publishedDate,
        interactionSettings, typeSpecificData
    );

    public static Voki CreateScoring(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds, VokiManagersIdsSet managers,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publishedDate,
        ScoringVokiInteractionSettings interactionSettings, ScoringVokiTypeSpecificData typeSpecificData
    ) => new(
        id, VokiType.Scoring, name, cover, primaryAuthorId, coAuthorIds, managers, details, tags, publishedDate,
        interactionSettings, typeSpecificData
    );

    public static Voki CreateTierList(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds, VokiManagersIdsSet managers,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publishedDate,
        TierListVokiInteractionSettings interactionSettings, TierListVokiTypeSpecificData typeSpecificData
    ) => new(
        id, VokiType.TierList, name, cover, primaryAuthorId, coAuthorIds, managers, details, tags, publishedDate,
        interactionSettings, typeSpecificData
    );

    public void UpdateVokiTakingsCount(uint newVokiTakingsCount) {
        if (newVokiTakingsCount > VokiTakingsCount) {
            VokiTakingsCount = newVokiTakingsCount;
        }
    }

    public void UpdateRatingsCount(uint newRatingsCount) {
        if (newRatingsCount > RatingsCount) {
            RatingsCount = newRatingsCount;
        }
    }
}