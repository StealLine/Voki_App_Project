using ApplicationShared;
using Microsoft.AspNetCore.Http;
using SharedKernel;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.user_ctx;

namespace InfrastructureShared.Auth;

public class UserCtxProvider : IUserCtxProvider
{
    public const string TokenCookieKey = "_token";
    public const string UserCtxHttpContextKey = "user_ctx_key";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenParser _tokenParser;

    public UserCtxProvider(IHttpContextAccessor httpContextAccessor, ITokenParser tokenParser) {
        _httpContextAccessor = httpContextAccessor;
        _tokenParser = tokenParser;
    }

    public IUserCtx Current =>
        TryGetSavedFromHttpCtx(out var ctx) ? ctx : UserIdFromToken();


    private IUserCtx UserIdFromToken() {
        if (!TryGetTokenFromCookie(out string? token) || string.IsNullOrEmpty(token)) {
            return new UnauthenticatedUserCtx(
                ErrFactory.AuthRequired("User is not authenticated", "Log in to your account")
            );
        }

        ErrOr<AppUserId> errOrUserId = _tokenParser.UserIdFromJwtToken(new JwtTokenString(token));
        if (errOrUserId.IsErr(out var err)) {
            return new UnauthenticatedUserCtx(err);
        }

        IUserCtx aCtx = IUserCtx.CreateAuthenticated(errOrUserId.AsSuccess());
        if (_httpContextAccessor.HttpContext is not null) {
            _httpContextAccessor.HttpContext.Items[UserCtxHttpContextKey] = aCtx;
        }

        return aCtx;
    }

    private bool TryGetTokenFromCookie(out string? token) {
        if (_httpContextAccessor.HttpContext is not null) {
            return _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(TokenCookieKey, out token);
        }

        token = null;
        return false;
    }

    private bool TryGetSavedFromHttpCtx(out IUserCtx userCtx) {
        if (
            _httpContextAccessor.HttpContext is not null
            && _httpContextAccessor.HttpContext.Items.TryGetValue(UserCtxHttpContextKey, out var ctxObj)
            && ctxObj is IUserCtx ctx
        ) {
            userCtx = ctx;
            return true;
        }

        userCtx = default!;
        return false;
    }
}