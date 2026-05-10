namespace Kuva.Auth.Business.Interfaces;

public interface IPasswordHashProvider
{
    string Algorithm { get; }
    string HashPassword(string password);
    bool VerifyPassword(string passwordHash, string password);
    bool NeedsRehash(string passwordHash);
}
