namespace SharedKernel.user_ctx;

public class AuthenticatedUserCtx : IUserCtx
{
    public AppUserId UserId { get; private set; }

    internal AuthenticatedUserCtx(AppUserId userId) {
        UserId = userId;
    }

    public bool IsAuthenticated(out AuthenticatedUserCtx aUserCtx) {
        aUserCtx = this;
        return true;
    }

    public ErrOr<AppUserId> TryGetUserId => UserId;
}