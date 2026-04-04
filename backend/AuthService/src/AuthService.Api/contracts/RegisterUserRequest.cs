using SharedKernel.common.app_users;

namespace AuthService.Api.contracts;

public class RegisterUserRequest : IRequestWithValidationNeeded
{
    public string UniqueName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }

    public ErrOrNothing Validate() {
        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (!Domain.common.Email.IsStringValidEmail(Email)) {
            errs.AddNext(ErrFactory.IncorrectFormat($"'{Email}' is not a invalid email"));
        }

        return errs
            .WithNextIfErr(PasswordRules.CheckForErr(Password))
            .WithNextIfErr(UserUniqueName.CheckForErr(UniqueName));
    }

    public UserUniqueName ParsedUniqueName => UserUniqueName.Create(UniqueName).AsSuccess();
    public Email ParsedEmail => Domain.common.Email.Create(Email).AsSuccess();
}