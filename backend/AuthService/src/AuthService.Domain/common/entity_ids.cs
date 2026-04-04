namespace AuthService.Domain.common;

public class UnconfirmedUserId(Guid value) : GuidBasedId(value)
{
    public static UnconfirmedUserId CreateNew() => new(Guid.CreateVersion7());
}
