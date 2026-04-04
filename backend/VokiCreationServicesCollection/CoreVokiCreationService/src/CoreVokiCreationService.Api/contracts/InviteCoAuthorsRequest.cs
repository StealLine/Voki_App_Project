namespace CoreVokiCreationService.Api.contracts;

public class InviteCoAuthorsRequest : IRequestWithValidationNeeded
{
    public string[] UserIds { get; init; }
    private const int MaxUserIdsCount = 50;

    public ErrOrNothing Validate() {
        if (UserIds == null || UserIds.Length == 0) {
            return ErrFactory.NoValue.Common("No user ids provided");
        }

        if (UserIds.Length > MaxUserIdsCount) {
            return ErrFactory.LimitExceeded(
                "Too many user ids provided",
                $"Cannot handle more than {MaxUserIdsCount} user ids at once. Current count is {UserIds.Length}"
            );
        }

        string[] invalidIds = UserIds
            .Where(x => !Guid.TryParse(x, out _))
            .ToArray();
        if (invalidIds.Length > 0) {
            return ErrFactory.IncorrectFormat(
                $"Some of provided user ids ({invalidIds.Length}) are invalid",
                $"Invalid ids: {string.Join(", ", invalidIds)}"
            );
        }

        ParsedUserIds = UserIds
            .Select(u => new AppUserId(new(u)))
            .ToImmutableHashSet();
        return ErrOrNothing.Nothing;
    }

    public ImmutableHashSet<AppUserId> ParsedUserIds { get; private set; }
}