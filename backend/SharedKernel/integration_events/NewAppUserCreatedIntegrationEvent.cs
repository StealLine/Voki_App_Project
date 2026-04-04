namespace SharedKernel.integration_events;
public record class NewAppUserCreatedIntegrationEvent(

    AppUserId CreatedUserId,
    string UserName
) : IIntegrationEvent;