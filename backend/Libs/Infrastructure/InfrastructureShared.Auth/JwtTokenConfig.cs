namespace InfrastructureShared.Auth;

public class JwtTokenConfig
{
    public string PublicKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string UserIdClaimKey { get; init; } = null!;
}