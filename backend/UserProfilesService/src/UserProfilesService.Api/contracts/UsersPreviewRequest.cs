namespace UserProfilesService.Api.contracts;

public class UsersPreviewRequest : IRequestWithValidationNeeded
{
    private const int MaxUserIdsCount = 150;
    public string[] UserIds { get; init; } = [];

    public ErrOrNothing Validate() {
        if (UserIds.Length > MaxUserIdsCount) {
            return ErrFactory.LimitExceeded($"Cannot process more than {MaxUserIdsCount} users at a time");
        }

        ParsedUserIds = UserIds
            .Where(i => Guid.TryParse(i, out _))
            .Select(i => new AppUserId(new Guid(i)))
            .ToImmutableHashSet();
        if (ParsedUserIds.Count == 0) {
            return ErrFactory.LimitExceeded(
                $"No valid user ids provided",
                details: $"Total ids provided: {UserIds.Length}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public ImmutableHashSet<AppUserId> ParsedUserIds { get; private set; }
}