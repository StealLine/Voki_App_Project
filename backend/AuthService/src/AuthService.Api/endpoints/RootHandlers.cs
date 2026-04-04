using AuthService.Application.app_users.queries;
using AuthService.Application.unconfirmed_users.commands;
using InfrastructureShared.Auth;
using SharedKernel;

namespace AuthService.Api.endpoints;

public class RootHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapPost("/ping", PingAuth);
        group.MapPost("/sign-up", RegisterUser)
            .WithRequestValidation<RegisterUserRequest>();
        group.MapPost("/login", LoginUser)
            .WithRequestValidation<LoginUserRequest>();
        group.MapPost("/confirm-registration", ConfirmUserRegistration)
            .WithRequestValidation<ConfirmRegistrationRequest>();
        group.MapPost("/logout", LogOutUser);
        
        return group;
    }

    private static async Task<IResult> PingAuth(
        IQueryHandler<PingUserAuthQuery, PingUserAuthQueryResult> handler,
        CancellationToken ct
    ) {
        PingUserAuthQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<PingUserAuthQueryResult, PingUserResponse>(result);
    }

    private static async Task<IResult> RegisterUser(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<RegisterUserCommand> handler
    ) {
        var request = httpContext.GetValidatedRequest<RegisterUserRequest>();

        RegisterUserCommand command = new(request.ParsedUniqueName, request.ParsedEmail, request.Password);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, Results.Created);
    }

    private static async Task<IResult> LoginUser(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetAuthTokenForAppUserQuery, JwtTokenString> handler
    ) {
        var request = httpContext.GetValidatedRequest<LoginUserRequest>();

        GetAuthTokenForAppUserQuery query = new(request.ParsedEmail, request.Password);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (token) => {
            httpContext.Response.Cookies.Append(UserCtxProvider.TokenCookieKey, token.Value, AuthCookieOptions());
            return Results.Ok();
        });
    }

    private static async Task<IResult> ConfirmUserRegistration(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<ConfirmUserRegistrationCommand, JwtTokenString> handler
    ) {
        var request = httpContext.GetValidatedRequest<ConfirmRegistrationRequest>();

        ConfirmUserRegistrationCommand command = new(request.ParsedUserId, request.ConfirmationCode);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (token) => {
            httpContext.Response.Cookies.Append(UserCtxProvider.TokenCookieKey, token.Value, AuthCookieOptions());
            return Results.Ok();
        });
    }

    private static IResult LogOutUser(HttpContext httpContext) {
        var cookieOptions = new CookieOptions {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        httpContext.Response.Cookies.Append(UserCtxProvider.TokenCookieKey, "", cookieOptions);
        return Results.Ok();
    }

    private static CookieOptions AuthCookieOptions() => new() {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddDays(30)
    };
}