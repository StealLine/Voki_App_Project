using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }

    public AppUser(AppUserId id) {
        Id = id;
        GeneralVokiTakenRecordIds = [];
        ReceivedResultIds = [];
    }

    public ImmutableHashSet<VokiTakenRecordId> GeneralVokiTakenRecordIds { get; private set; }
    public ImmutableHashSet<GeneralVokiResultId> ReceivedResultIds { get; set; }

    public void AddVokiTaken(VokiTakenRecordId id, GeneralVokiResultId receivedResultId) {
        GeneralVokiTakenRecordIds = GeneralVokiTakenRecordIds.Add(id);
        ReceivedResultIds = ReceivedResultIds.Add(receivedResultId);
    }
}