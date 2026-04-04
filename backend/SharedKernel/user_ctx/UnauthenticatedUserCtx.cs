namespace SharedKernel.user_ctx;

public sealed class UnauthenticatedUserCtx : IUserCtx
{
    public UnauthenticatedUserCtx(Err? authErr =null ) {
        AuthErr = authErr ?? ErrFactory.AuthRequired("User is not authenticated");
    }

    public ErrOr<AppUserId> TryGetUserId => AuthErr;
    public Err AuthErr { get; }
    public bool IsAuthenticated(out AuthenticatedUserCtx aUserCtx) {
        aUserCtx = null!;
        return false;
    }
}