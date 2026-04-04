using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.domain;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

internal class CoAuthorInviteAcceptedEventHandler : IDomainEventHandler<CoAuthorInviteAcceptedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public CoAuthorInviteAcceptedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(CoAuthorInviteAcceptedEvent e, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetByIdForUpdate(e.AppUserId, ct))!;
        var res = user.AcceptCoAuthorInvite(e.VokiId);
        UnexpectedBehaviourException.ThrowIfErr(res);
       await _appUsersRepository.Update(user, ct);
    }
}