namespace GeneralVokiTakingService.Domain.common;

public class VokiTakingSessionId(Guid value) : GuidBasedId(value)
{
    public static VokiTakingSessionId CreateNew() => new(Guid.CreateVersion7());
}
