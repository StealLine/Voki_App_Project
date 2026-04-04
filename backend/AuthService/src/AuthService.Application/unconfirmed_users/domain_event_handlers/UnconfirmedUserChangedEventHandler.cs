using AuthService.Application.abstractions;
using AuthService.Domain.unconfirmed_user_aggregate.events;
using SharedKernel.domain;


namespace AuthService.Application.unconfirmed_users.domain_event_handlers;

internal class UnconfirmedUserChangedEventHandler : IDomainEventHandler<UnconfirmedUserChangedEvent>
{
    private readonly IEmailService _emailService;

    public UnconfirmedUserChangedEventHandler(IEmailService emailService) {
        _emailService = emailService;
    }

    public async Task Handle(UnconfirmedUserChangedEvent e, CancellationToken ct) {
        ErrOrNothing sendingErr = await _emailService.SendRegistrationConfirmationLink(
            e.Email, e.UniqueName, e.UserId, e.ConfirmationCode, e.ExpirationDate
        );

        UnexpectedBehaviourException.ThrowIfErr(
            sendingErr, "Unable to send email confirmation link. Please try again later"
        );
    }
}