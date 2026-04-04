using SharedKernel.common.app_users;

namespace AuthService.Domain.app_user_aggregate.events;

public record NewAppUserCreatedEvent(
    AppUserId CreatedUserId,
    UserUniqueName UserUniqueName,
    DateTime RegistrationDate
) : IDomainEvent;