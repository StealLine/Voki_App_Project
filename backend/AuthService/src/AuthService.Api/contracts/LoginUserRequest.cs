namespace AuthService.Api.contracts;

internal class LoginUserRequest : IRequestWithValidationNeeded
{
    public string Email { get; init; }
    public string Password { get; init; }

    public ErrOrNothing Validate() {
        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (string.IsNullOrWhiteSpace(Email)) {
            errs.AddNext(ErrFactory.NoValue.Common("Email is required"));
        }

        else if (!Domain.common.Email.IsStringValidEmail(Email)) {
            errs.AddNext(ErrFactory.IncorrectFormat("Incorrect email format"));
        }


        if (string.IsNullOrWhiteSpace(Password)) {
            return ErrFactory.NoValue.Common("Password is required");
        }

        return errs;
    }

    public Email ParsedEmail => Domain.common.Email.Create(Email).AsSuccess();
}