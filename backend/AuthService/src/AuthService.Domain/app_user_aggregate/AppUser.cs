using AuthService.Domain.app_user_aggregate.events;
using SharedKernel.common.app_users;

namespace AuthService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public Email Email { get; }
    public UserUniqueName UniqueName { get; }
    public string PasswordHash { get; }
    public DateTime RegistrationDate { get; }
    public DateTime PasswordUpdateDate { get; private set; }

    private AppUser(
        AppUserId id, Email email, UserUniqueName uniqueName,
        string passwordHash, DateTime registrationDate
    ) {
        Id = id;
        Email = email;
        UniqueName = uniqueName;
        PasswordHash = passwordHash;
        RegistrationDate = registrationDate;
        PasswordUpdateDate = registrationDate;
    }

    public bool IsPasswordCorrect(IPasswordHasher passwordHasher, string password) {
        return passwordHasher.Verify(
            passwordToCheck: password,
            passwordHash: PasswordHash
        );
    }

    public static AppUser CreateNew(
        UnconfirmedUserId unconfirmedUserId, Email email, string passwordHash,
        UserUniqueName userUniqueName, DateTime now
    ) {
        AppUserId userId = new AppUserId(unconfirmedUserId.Value);
        AppUser user = new(userId, email, userUniqueName, passwordHash, now);
        user.AddDomainEvent(new NewAppUserCreatedEvent(user.Id, userUniqueName, user.RegistrationDate));
        return user;
    }
}