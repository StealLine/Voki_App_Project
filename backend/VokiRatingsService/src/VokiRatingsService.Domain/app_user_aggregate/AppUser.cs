namespace VokiRatingsService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiRatingId> RatingIds { get; private set; }
    private ImmutableHashSet<VokiId> TakenVokiIds { get; set; }


    public AppUser(AppUserId id) {
        Id = id;
        RatingIds = [];
        TakenVokiIds = [];
    }

    public bool AddTakenVoki(VokiId vokiId) {
        if (TakenVokiIds.Contains(vokiId)) {
            return false;
        }

        TakenVokiIds = TakenVokiIds.Add(vokiId);
        return true;
    }

    public bool HasTaken(VokiId vokiId) => TakenVokiIds.Contains(vokiId);

    public void AddRating(VokiRatingId ratingId) {
        RatingIds = RatingIds.Add(ratingId);
    }
}