using AuthService.Application.app_users.queries;

namespace AuthService.Api.contracts;

public record class PingUserResponse(
    string UserId,
    bool IsAuthenticated
) : ICreatableResponse<PingUserAuthQueryResult>
{
    public static ICreatableResponse<PingUserAuthQueryResult> Create(PingUserAuthQueryResult res) => res.Switch(
        authenticated: (userId) => new PingUserResponse(userId.ToString(), true),
        unauthenticated: () => new PingUserResponse("", false)
    );
}