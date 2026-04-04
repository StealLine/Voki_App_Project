namespace SharedKernel.user_ctx;

public interface IUserCtx
{
    public bool IsAuthenticated(out AuthenticatedUserCtx aUserCtx);
    public ErrOr<AppUserId> TryGetUserId { get; }

    public TResult Match<TResult>(
        Func<AuthenticatedUserCtx, TResult> authenticatedFunc,
        Func<UnauthenticatedUserCtx, TResult> unauthenticatedFunc
    ) =>
        this is AuthenticatedUserCtx authenticatedUser ? authenticatedFunc(authenticatedUser) :
        this is UnauthenticatedUserCtx unauthenticatedUser ? unauthenticatedFunc(unauthenticatedUser) :
        throw new InvalidOperationException($"Unknown type of {nameof(IUserCtx)}: {GetType().Name}");

    public static sealed IUserCtx CreateAuthenticated(AppUserId id) => new AuthenticatedUserCtx(id);
}