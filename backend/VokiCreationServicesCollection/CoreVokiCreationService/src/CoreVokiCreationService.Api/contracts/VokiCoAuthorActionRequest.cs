namespace CoreVokiCreationService.Api.contracts;

public class VokiCoAuthorActionRequest : IRequestWithValidationNeeded
{
    public string UserId { get; init; } = "";

    public ErrOrNothing Validate() =>
        Guid.TryParse(UserId, out var _)
            ? ErrOrNothing.Nothing
            : ErrFactory.IncorrectFormat(
                "Incorrect new user id format",
                $"Given value: {UserId} is not in a correct format for user id"
            );

    public AppUserId ParsedUserId => new(new Guid(UserId));
}