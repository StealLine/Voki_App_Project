namespace AuthService.Domain.rules;

public static class PasswordRules
{
    public const int MinLength = 8;
    public const int MaxLength = 20;

    public static readonly string IncorrectLengthMessage =
        $"Password must be between {MinLength} and {MaxLength} characters";

    public static readonly string MustContainLetterMessage = "Password must contain at least one letter";
    public static readonly string MustContainNumberMessage = "Password must contain at least one number";

    public static ErrOrNothing CheckForErr(string password) {
        ErrOrNothing err = ErrOrNothing.Nothing;

        int passwordLength = string.IsNullOrEmpty(password) ? 0 : password.Length;
        if (passwordLength < MinLength || passwordLength > MaxLength) {
            err.AddNext(ErrFactory.IncorrectFormat(IncorrectLengthMessage));
        }

        if (!password.Any(char.IsLetter)) {
            err.AddNext(ErrFactory.IncorrectFormat(MustContainLetterMessage));
        }

        if (!password.Any(char.IsDigit)) {
            err.AddNext(ErrFactory.IncorrectFormat(MustContainNumberMessage));
        }

        return err;
    }
}