using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.app_users.commands;

public sealed record UpdateAutoAlbumsAppearanceCommand(
    UserAutoAlbumsAppearance NewAppearance
) :
    ICommand<UserAutoAlbumsAppearance>,
    IWithAuthCheckStep;

internal sealed class UpdateAutoAlbumsAppearanceCommandHandler :
    ICommandHandler<UpdateAutoAlbumsAppearanceCommand, UserAutoAlbumsAppearance>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IAppUsersRepository _appUsersRepository;

    public UpdateAutoAlbumsAppearanceCommandHandler(IUserCtxProvider userCtxProvider, IAppUsersRepository appUsersRepository) {
        _userCtxProvider = userCtxProvider;
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<UserAutoAlbumsAppearance>> Handle(UpdateAutoAlbumsAppearanceCommand command, CancellationToken ct) {
        AppUser? user = await _appUsersRepository.GetCurrentForUpdate(command.UserCtx(_userCtxProvider), ct);
        if (user is null) {
            return ErrFactory.NotFound.User("Couldn't find user to update albums appearance");
        }

        user.UpdateAutoAlbumsAppearance(command.NewAppearance);
        await _appUsersRepository.Update(user, ct);
        return user.AutoAlbumsAppearance;
    }
}