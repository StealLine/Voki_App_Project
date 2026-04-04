using SharedKernel.domain;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;
using VokisCatalogService.Domain.voki_aggregate.events;

namespace VokisCatalogService.Application.app_users.domain_event_handlers;

internal class PublishedVokiCreatedEventHandler : IDomainEventHandler<PublishedVokiCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public PublishedVokiCreatedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(PublishedVokiCreatedEvent e, CancellationToken ct) {
        List<AppUser> usersToUpdate = [];
        AppUser? initiator = await _appUsersRepository.GetByIdForUpdate(e.PrimaryAuthorId, ct);
        if (initiator is not null) {
            initiator.AddInitializedVoki(e.VokiId);
            usersToUpdate.Add(initiator);
        }

        foreach (var eCoAuthorId in e.CoAuthorIds) {
            AppUser? coAuthor = await _appUsersRepository.GetByIdForUpdate(eCoAuthorId, ct);
            if (coAuthor is not null) {
                coAuthor.AddCoAuthoredVoki(e.VokiId);
                usersToUpdate.Add(coAuthor);
            }
        }

        await _appUsersRepository.UpdateRange(usersToUpdate, ct);
    }
}