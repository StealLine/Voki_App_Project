namespace SharedKernel;

public record JwtTokenString(string Value)
{
    public override string ToString() => Value;
}