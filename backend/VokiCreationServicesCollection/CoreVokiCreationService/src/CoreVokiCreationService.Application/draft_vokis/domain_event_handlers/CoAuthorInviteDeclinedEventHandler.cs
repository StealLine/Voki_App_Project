using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate.events;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.domain;

namespace CoreVokiCreationService.Application.draft_vokis.domain_event_handlers;

internal class CoAuthorInviteDeclinedEventHandler : IDomainEventHandler<CoAuthorInviteDeclinedEvent>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    public CoAuthorInviteDeclinedEventHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task Handle(CoAuthorInviteDeclinedEvent e, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(e.VokiId, ct))!;
        voki.DeclineCoAuthorInvite(e.AppUserId);
        await _draftVokiRepository.Update(voki, ct);
    }
}