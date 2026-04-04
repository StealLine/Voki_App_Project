using GeneralVokiTakingService.Domain.common;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.continue_taking;

public class ContinueVokiTakingRequest : IRequestWithValidationNeeded
{
    public string SessionId { get; init; }

    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SessionId) || !Guid.TryParse(SessionId, out var sessionGuid)) {
            return ErrFactory.IncorrectFormat("Provided session id is invalid");
        }

        ParsedSessionId = new VokiTakingSessionId(sessionGuid);
        return ErrOrNothing.Nothing;
    }
    public VokiTakingSessionId ParsedSessionId { get; private set; }

}