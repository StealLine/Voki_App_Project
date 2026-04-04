using AuthService.Application.abstractions;
using AuthService.Application.common.repositories;
using AuthService.Domain.app_user_aggregate;

namespace AuthService.Application.app_users.queries;

public sealed record GetAuthTokenForAppUserQuery(
    Email Email,
    string Password
) : IQuery<JwtTokenString>;

internal sealed class GetAuthTokenForAppUserQueryHandler : IQueryHandler<GetAuthTokenForAppUserQuery, JwtTokenString>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;

    public GetAuthTokenForAppUserQueryHandler(
        IAppUsersRepository appUsersRepository,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator
    ) {
        _appUsersRepository = appUsersRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ErrOr<JwtTokenString>> Handle(GetAuthTokenForAppUserQuery query, CancellationToken ct) {
        AppUser? user = await _appUsersRepository.GetByEmail(query.Email, ct);
        if (user is null) {
            return ErrFactory.NotFound.User();
        }

        bool isPasswordCorrect = user.IsPasswordCorrect(_passwordHasher, query.Password);
        if (!isPasswordCorrect) {
            return ErrFactory.NoAccess("Incorrect password");
        }

        return _tokenGenerator.CreateToken(user);
    }
}