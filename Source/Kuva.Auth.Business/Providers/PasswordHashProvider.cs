using Kuva.Auth.Business.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kuva.Auth.Business.Providers;

public sealed class PasswordHashProvider : IPasswordHashProvider
{
    private readonly PasswordHasher<object> _hasher = new();
    private static readonly object User = new();

    public string Algorithm => "MicrosoftPasswordHasherV3";

    public string HashPassword(string password) => _hasher.HashPassword(User, password);

    public bool VerifyPassword(string passwordHash, string password)
    {
        var result = _hasher.VerifyHashedPassword(User, passwordHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }

    public bool NeedsRehash(string passwordHash) =>
        _hasher.VerifyHashedPassword(User, passwordHash, string.Empty) == PasswordVerificationResult.SuccessRehashNeeded;
}
