using System.Security.Cryptography;
using Kuva.Auth.Business.Interfaces;

namespace Kuva.Auth.Business.Providers;

public sealed class TokenHashProvider : ITokenHashProvider
{
    public string GenerateToken(int bytes)
    {
        var buffer = RandomNumberGenerator.GetBytes(bytes);
        return Base64UrlEncode(buffer);
    }

    public string HashToken(string token)
    {
        var hash = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash);
    }

    private static string Base64UrlEncode(byte[] input) =>
        Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
