namespace AuthService.Domain.common;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string passwordToCheck, string passwordHash);
}
