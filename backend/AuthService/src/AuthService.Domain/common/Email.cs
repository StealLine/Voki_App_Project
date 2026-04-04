using System.Text.RegularExpressions;
using SharedKernel.exceptions;

namespace AuthService.Domain.common;

public class Email : ValueObject
{
    private string _email;

    private Email(string email)
    {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(email));
        _email = email;
    }

    private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public override string ToString() => _email;
    public override IEnumerable<object> GetEqualityComponents() => [_email];

    public static ErrOr<Email> Create(string email) =>
        CheckForErr(email).IsErr(out var err) ? err : new Email(email);

    public static ErrOrNothing CheckForErr(string? email)
    {
        if (!IsStringValidEmail(email))
        {
            return ErrFactory.IncorrectFormat("Invalid email format");
        }

        return ErrOrNothing.Nothing;
    }

    public static bool IsStringValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, EmailRegex);
}