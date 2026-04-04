using MassTransit;
using SharedKernel.common.app_users;
using SharedKernel.integration_events;
using UserProfilesService.Application.common;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Application.app_users.integration_event_handlers;

public class NewAppUserCreatedIntegrationEventHandler : IConsumer<NewAppUserCreatedIntegrationEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public NewAppUserCreatedIntegrationEventHandler(
        IAppUsersRepository appUsersRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _appUsersRepository = appUsersRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task Consume(ConsumeContext<NewAppUserCreatedIntegrationEvent> context) {
        ErrOr<UserProfilePicKey> picRes = await _mainStorageBucket.CopyUserProfilePicFromPresets(
            presetKey: PresetProfilePicKey.DefaultProfilePic,
            context.Message.CreatedUserId,
            context.CancellationToken
        );
        if (picRes.IsErr(out var err)) {
            return;
        }

        AppUser user = new AppUser(
            context.Message.CreatedUserId,
            new UserUniqueName(context.Message.UserName),
            picRes.AsSuccess()
        );
        await _appUsersRepository.Add(user, context.CancellationToken);
    }
}