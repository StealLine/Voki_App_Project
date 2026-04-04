using SharedKernel.user_ctx;

namespace VokiRatingsService.Domain.voki_aggregate;

public class Voki : AggregateRoot<VokiId>
{
    private Voki() { }
    public AppUserId PrimaryAuthorId { get; }
    public VokiManagersIdsSet ManagersSet { get; private set; }
    public DateTime PublicationDate { get; }

    public Voki(VokiId id, AppUserId primaryAuthorId, VokiManagersIdsSet managers, DateTime publicationDate) {
        Id = id;
        PrimaryAuthorId = primaryAuthorId;
        ManagersSet = managers;
        PublicationDate = publicationDate;
    }

    public bool CanUserManage(AuthenticatedUserCtx userContext) =>
        CanUserManage(userContext, PrimaryAuthorId, ManagersSet);

    public static bool CanUserManage(
        AuthenticatedUserCtx userContext,
        AppUserId primaryAuthorId,
        VokiManagersIdsSet managersSet
    ) {
        AppUserId userId = userContext.UserId;
        return primaryAuthorId == userId || managersSet.Contains(userId);
    }
}