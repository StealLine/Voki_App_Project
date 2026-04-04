using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace InfrastructureShared.Auth;

internal class TokenParser : ITokenParser
{
    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
    private readonly ILogger<TokenParser> _logger;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _userIdClaimKey;
    private readonly RsaSecurityKey _publicKey;

    public TokenParser(JwtTokenConfig jwtTokenConfig, ILogger<TokenParser> logger) {
        _logger = logger;

        _issuer = jwtTokenConfig.Issuer;
        _audience = jwtTokenConfig.Audience;
        _userIdClaimKey = jwtTokenConfig.UserIdClaimKey;

        RSA rsa = RSA.Create();
        rsa.ImportFromPem(jwtTokenConfig.PublicKey);
        _publicKey = new RsaSecurityKey(rsa);
    }


    public ErrOr<AppUserId> UserIdFromJwtToken(JwtTokenString token) {
        var parameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = _publicKey
        };

        try {
            ClaimsPrincipal principal = JwtSecurityTokenHandler.ValidateToken(
                token.Value,
                parameters,
                out _
            );

            Claim? userIdClaim = principal.Claims.FirstOrDefault(c => c.Type ==_userIdClaimKey);

            if (userIdClaim is null) {
                _logger.LogWarning(
                    "JWT token does not contain '{claimName}' claim. Token: '{token}'.",
                    _userIdClaimKey, token.Value
                );

                return ErrFactory.IncorrectFormat(
                    "Invalid authentication token",
                    details: "Token does not contain required data"
                );
            }

            string claimValue = userIdClaim.Value;

            if (string.IsNullOrWhiteSpace(claimValue)) {
                _logger.LogWarning(
                    "JWT token contains empty '{claimName}' claim. Token: '{token}'.",
                    _userIdClaimKey, token.Value
                );

                return ErrFactory.NoValue.Common(
                    "Invalid authentication token",
                    details: "User id inside token is missing"
                );
            }

            if (!Guid.TryParse(claimValue, out Guid parsedGuid)) {
                _logger.LogWarning(
                    "JWT token contains malformed userId claim. Value: '{claimValue}'. Token: '{token}'.",
                    claimValue, token.Value
                );

                return ErrFactory.IncorrectFormat(
                    "Invalid authentication token",
                    details: "User id inside token has incorrect format"
                );
            }

            return new AppUserId(parsedGuid);
        }
        catch (Exception ex) {
            _logger.LogError(
                ex,
                "Unexpected error while parsing JWT token. Token: '{token}'. Error: {errorMessage}.",
                token.Value, ex.Message
            );

            return ErrFactory.AuthRequired(
                "Authentication failed",
                details: "Unexpected error occurred while validating token"
            );
        }
    }
}