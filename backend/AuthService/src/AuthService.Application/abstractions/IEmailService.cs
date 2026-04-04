using SharedKernel.common.app_users;

namespace AuthService.Application.abstractions;

public interface IEmailService
{
    Task<ErrOrNothing> SendRegistrationConfirmationLink(
        Email email,
        UserUniqueName userUniqueName,
        UnconfirmedUserId userId,
        string confirmationCode,
        DateTime expirationDate
    );
}