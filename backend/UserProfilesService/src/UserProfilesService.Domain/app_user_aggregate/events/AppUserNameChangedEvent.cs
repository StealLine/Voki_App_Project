using SharedKernel.common.app_users;

namespace UserProfilesService.Domain.app_user_aggregate.events;

public sealed record class AppUserNameChangedEvent(
    AppUserId UserId,
    UserUniqueName NewUniqueName
) : IDomainEvent;