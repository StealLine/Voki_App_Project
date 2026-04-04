using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.domain;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

internal class VokiCoAuthorRemovedEventHandler : IDomainEventHandler<VokiCoAuthorRemovedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public VokiCoAuthorRemovedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(VokiCoAuthorRemovedEvent e, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetByIdForUpdate(e.AppUserId, ct))!;
        user.RemoveCoAuthoredVoki(e.VokiId);
        await _appUsersRepository.Update(user, ct);
    }
}