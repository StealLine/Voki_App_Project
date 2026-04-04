namespace AuthService.Api.contracts;

internal class ConfirmRegistrationRequest : IRequestWithValidationNeeded
{
    public string UserId { get; init; }
    public string ConfirmationCode { get; init; }

    public ErrOrNothing Validate() {
        if (!Guid.TryParse(UserId, out var _)) {
            return ErrFactory.IncorrectFormat(
                "Incorrect confirmation data", "Provided user id is of incorrect format"
            );
        }

        if (string.IsNullOrWhiteSpace(ConfirmationCode)) {
            return ErrFactory.NoValue.Common("Incorrect confirmation data", "Provided confirmation code is empty");
        }

        return ErrOrNothing.Nothing;
    }

    public UnconfirmedUserId ParsedUserId => new(Guid.Parse(UserId));
}