

using SharedKernel.common.app_users;

namespace AuthService.Domain.unconfirmed_user_aggregate.events;

public record UnconfirmedUserChangedEvent(
    UnconfirmedUserId UserId,
    UserUniqueName UniqueName,
    Email Email,
    string ConfirmationCode,
    DateTime ExpirationDate
) : IDomainEvent;