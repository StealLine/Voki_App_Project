using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using SharedKernel.errs.utils;

namespace AuthService.Infrastructure.auth;

internal sealed class PasswordHasher : IPasswordHasher
{
    private const int CurrentSaltSize = 16; // 128-bit
    private const int CurrentHashSize = 32; // 256-bit
    private const int CurrentIterations = 1235;
    private static readonly HashAlgorithmName CurrentAlg = HashAlgorithmName.SHA512;
    private const string Version = "v1";
    private const string Kdf = "pbkdf2";

    private readonly ILogger<PasswordHasher> _logger;

    public PasswordHasher(ILogger<PasswordHasher> logger) {
        _logger = logger;
    }

    public string Hash(string password) {
        byte[] salt = RandomNumberGenerator.GetBytes(CurrentSaltSize);
        byte[] derived = Derive(password, salt, CurrentIterations, CurrentAlg, CurrentHashSize);

        string algName = (CurrentAlg.Name ?? "sha512").ToLowerInvariant();

        // v1|pbkdf2|sha512|i=1235|l=32|s=BASE64|h=BASE64
        return string.Join('|',
            Version,
            Kdf,
            algName,
            $"i={CurrentIterations}",
            $"l={CurrentHashSize}",
            $"s={Convert.ToBase64String(salt)}",
            $"h={Convert.ToBase64String(derived)}"
        );
    }

    public bool Verify(string password, string passwordHash) {
        ErrOr<ParsedHash> parseResult = TryParse(passwordHash);

        if (parseResult.IsErr(out var err)) {
            _logger.LogWarning("Password hash parse failed: {Msg}", err);
            return false;
        }

        ParsedHash p = parseResult.AsSuccess();

        var calc = Derive(password, p.Salt, p.Iterations, p.Alg, p.HashLen);
        return CryptographicOperations.FixedTimeEquals(calc, p.Hash);
    }

    private static byte[] Derive(string password, byte[] salt, int iterations, HashAlgorithmName alg, int hashLen)
        => Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, alg, hashLen);

    private static ErrOr<ParsedHash> TryParse(string s) {
        if (string.IsNullOrWhiteSpace(s)) {
            return ErrFactory.IncorrectFormat("Password hash is empty or whitespace.");
        }

        string[] parts = s.Split('|');
        if (parts.Length != 7) {
            return ErrFactory.IncorrectFormat($"Unexpected parts count: {parts.Length} (expected 7).");
        }

        string version = parts[0];
        string kdf = parts[1];
        string algStr = parts[2];

        if (!algStr.Equals("sha512", StringComparison.OrdinalIgnoreCase)) {
            return ErrFactory.IncorrectFormat($"Unsupported algorithm: {algStr}");
        }

        HashAlgorithmName alg = HashAlgorithmName.SHA512;

        if (!parts[3].StartsWith("i=", StringComparison.Ordinal) ||
            !int.TryParse(parts[3].AsSpan(2), out int iterations) || iterations <= 0) {
            return ErrFactory.IncorrectFormat($"Invalid iterations segment: {parts[3]}");
        }

        if (!parts[4].StartsWith("l=", StringComparison.Ordinal) ||
            !int.TryParse(parts[4].AsSpan(2), out int hashLen) || hashLen <= 0) {
            return ErrFactory.IncorrectFormat($"Invalid hash length segment: {parts[4]}");
        }

        if (!parts[5].StartsWith("s=", StringComparison.Ordinal)) {
            return ErrFactory.IncorrectFormat($"Invalid salt segment prefix: {parts[5]}");
        }

        byte[] salt;
        try {
            salt = Convert.FromBase64String(parts[5].Substring(2));
        }
        catch (Exception ex) {
            return ErrFactory.IncorrectFormat($"Salt base64 parse error: {ex.Message}");
        }

        if (!parts[6].StartsWith("h=", StringComparison.Ordinal)) {
            return ErrFactory.IncorrectFormat($"Invalid hash segment prefix: {parts[6]}");
        }

        try {
            byte[] hash = Convert.FromBase64String(parts[6].Substring(2));
            return new ParsedHash(version, kdf, alg, iterations, hashLen, salt, hash);
        }
        catch (Exception ex) {
            return ErrFactory.IncorrectFormat($"Hash base64 parse error: {ex.Message}");
        }
    }

    private readonly record struct ParsedHash(
        string Version,
        string Kdf,
        HashAlgorithmName Alg,
        int Iterations,
        int HashLen,
        byte[] Salt,
        byte[] Hash
    );
}