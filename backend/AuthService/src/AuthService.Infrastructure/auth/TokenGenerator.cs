using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Application.abstractions;
using AuthService.Domain.app_user_aggregate;
using InfrastructureShared.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.auth;

internal sealed class TokenGenerator : ITokenGenerator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _userIdClaimKey;

    private readonly RsaSecurityKey _privateKey;
    private readonly ILogger<TokenGenerator> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TokenGenerator(
        JwtTokenConfig jwtTokenConfig,
        AuthPrivateKeyConfig privateKeyConfig,
        ILogger<TokenGenerator> logger,
        IDateTimeProvider dateTimeProvider
    ) {
        _issuer = jwtTokenConfig.Issuer;
        _audience = jwtTokenConfig.Audience;
        _userIdClaimKey = jwtTokenConfig.UserIdClaimKey;

        RSA rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyConfig.PrivateKey);
        _privateKey = new RsaSecurityKey(rsa);

        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }


    public JwtTokenString CreateToken(AppUser user) {
        try {
            Claim[] claims = [new(_userIdClaimKey, user.Id.ToString())];
            SigningCredentials creds = new(_privateKey, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: _dateTimeProvider.UtcNow.AddDays(30),
                signingCredentials: creds
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtTokenString(tokenString);
        }
        catch (Exception ex) {
            _logger.LogError(
                ex,
                "Failed to generate JWT token for userId '{userId}'. Error: {errorMessage}",
                user.Id.ToString(),
                ex.Message
            );

            throw;
        }
    }
}