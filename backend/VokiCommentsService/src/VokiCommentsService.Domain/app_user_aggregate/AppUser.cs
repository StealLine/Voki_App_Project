namespace VokiCommentsService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiCommentId> CommentIds { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        CommentIds = [];
    }
}