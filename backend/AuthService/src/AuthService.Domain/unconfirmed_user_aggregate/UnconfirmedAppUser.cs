using AuthService.Domain.rules;
using AuthService.Domain.unconfirmed_user_aggregate.events;
using SharedKernel.common.app_users;

namespace AuthService.Domain.unconfirmed_user_aggregate;

public class UnconfirmedUser : AggregateRoot<UnconfirmedUserId>
{
    private UnconfirmedUser() { }
    public UserUniqueName UserUniqueName { get; private set; }
    public Email Email { get; }
    public string PasswordHash { get; private set; }
    private string ConfirmationCode { get; }
    public DateTime ExpiresAt { get; private set; }

    private UnconfirmedUser(
        UnconfirmedUserId id,
        UserUniqueName userUniqueName,
        Email email,
        string passwordHash,
        string confirmationCode,
        DateTime expiresAt
    ) {
        Id = id;
        UserUniqueName = userUniqueName;
        Email = email;
        PasswordHash = passwordHash;
        ConfirmationCode = confirmationCode;
        ExpiresAt = expiresAt;
    }

    public static readonly TimeSpan ConfirmationPeriod = TimeSpan.FromMinutes(5);

    public static ErrOr<UnconfirmedUser> CreateNew(
        UserUniqueName uniqueName, Email email, DateTime now,
        string password, IPasswordHasher passwordHasher
    ) {
        if (PasswordRules.CheckForErr(password).IsErr(out var err)) {
            return err;
        }

        DateTime expiresAt = now + ConfirmationPeriod;

        UnconfirmedUser user = new(
            UnconfirmedUserId.CreateNew(),
            uniqueName,
            email,
            passwordHash: passwordHasher.Hash(password),
            confirmationCode: Guid.NewGuid().ToString(),
            expiresAt
        );
        user.AddDomainEvent(new UnconfirmedUserChangedEvent(
            user.Id, user.UserUniqueName, user.Email, user.ConfirmationCode, user.ExpiresAt
        ));
        return user;
    }

    public ErrOrNothing Override(
        UserUniqueName userUniqueName,
        string password,
        IPasswordHasher passwordHasher,
        DateTime utcNow
    ) {
        if (PasswordRules.CheckForErr(password).IsErr(out var err)) {
            return err;
        }

        this.PasswordHash = passwordHasher.Hash(password);
        this.UserUniqueName = userUniqueName;
        this.ExpiresAt = utcNow + ConfirmationPeriod;

        this.AddDomainEvent(new UnconfirmedUserChangedEvent(Id, UserUniqueName, Email, ConfirmationCode, ExpiresAt));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing TryConfirm(string confirmationCode) {
        if (confirmationCode != this.ConfirmationCode) {
            return ErrFactory.Unspecified("Unable to confirm user. Incorrect confirmation data was provided");
        }

        return ErrOrNothing.Nothing;
    }
}