using AuthService.Domain.app_user_aggregate;

namespace AuthService.Application.abstractions;

public interface ITokenGenerator
{
    public JwtTokenString CreateToken(AppUser user);
}