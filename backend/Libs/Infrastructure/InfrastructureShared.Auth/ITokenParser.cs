using SharedKernel;
using SharedKernel.domain.ids;
using SharedKernel.errs;

namespace InfrastructureShared.Auth;

public interface ITokenParser
{
    public ErrOr<AppUserId> UserIdFromJwtToken(JwtTokenString token);

}
