using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Entities.Dtos.Responses;

namespace Kuva.Auth.Business.Providers;

public sealed class JwksProvider(IJwtKeyProvider keyProvider) : IJwksProvider
{
    public JwksResponse GetJwks()
    {
        var signingKey = keyProvider.GetSigningKey();
        var parameters = signingKey.Rsa.ExportParameters(false);
        return new JwksResponse(new[]
        {
            new JwksKeyResponse(
                "RSA",
                "sig",
                signingKey.KeyId,
                "RS256",
                Base64Url(parameters.Modulus!),
                Base64Url(parameters.Exponent!))
        });
    }

    private static string Base64Url(byte[] value) => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
