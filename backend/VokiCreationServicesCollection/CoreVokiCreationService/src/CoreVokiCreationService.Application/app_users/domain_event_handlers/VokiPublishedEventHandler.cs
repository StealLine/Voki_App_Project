using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.domain;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

internal class VokiPublishedEventHandler : IDomainEventHandler<VokiPublishedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public VokiPublishedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(VokiPublishedEvent e, CancellationToken ct) {
        List<AppUser> usersTpUpdate = [];

        AppUser? initiator = await _appUsersRepository.GetByIdForUpdate(e.PrimaryAuthorId, ct);
        if (initiator is not null) {
            initiator.RemoveInitializedVoki(e.VokiId);
            usersTpUpdate.Add(initiator);
        }

        foreach (var coAuthorsId in e.CoAuthorsIds) {
            AppUser? coauthor = await _appUsersRepository.GetByIdForUpdate(coAuthorsId, ct)!;
            if (coauthor is not null) {
                coauthor.RemoveCoAuthoredVoki(e.VokiId);
                usersTpUpdate.Add(coauthor);
            }
        }

        if (usersTpUpdate.Count > 0) {
            await _appUsersRepository.UpdateRange(usersTpUpdate, ct);
        }
    }
}