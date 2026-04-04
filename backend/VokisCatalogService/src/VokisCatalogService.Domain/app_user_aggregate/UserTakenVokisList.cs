namespace VokisCatalogService.Domain.app_user_aggregate;

public class UserTakenVokisList : Entity<UserTakenVokisListId>
{
    private UserTakenVokisList() { }
    public ImmutableDictionary<VokiId, UserTakenVokiData> Value { get; private set; }

    public static UserTakenVokisList CreateNew() => new() {
        Id = UserTakenVokisListId.CreateNew(),
        Value = ImmutableDictionary<VokiId, UserTakenVokiData>.Empty,
    };

    public void Add(VokiId vokiId, DateTime currentTime) {
        if (Value.TryGetValue(vokiId, out var data)) {
            Value = Value.SetItem(vokiId, new UserTakenVokiData(data.TimesTaken + 1, currentTime));
        }
        else {
            Value = Value.SetItem(vokiId, new UserTakenVokiData(1, currentTime));
        }
    }
}

public record UserTakenVokiData(uint TimesTaken, DateTime LastTimeTaken);