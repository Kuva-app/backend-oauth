using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Entities.Dtos.Responses;

namespace Kuva.Auth.Business.Providers;

public sealed class JwksProvider(IJwtKeyProvider keyProvider) : IJwksProvider
{
    public JwksResponse GetJwks()
    {
        var signingKey = keyProvider.GetSigningKey();
        var parameters = signingKey.Ecdsa.ExportParameters(false);
        return new JwksResponse(new[]
        {
            new JwksKeyResponse(
                "EC",
                "sig",
                signingKey.KeyId,
                "ES256",
                Base64Url(parameters.Q.X!),
                Base64Url(parameters.Q.Y!))
        });
    }

    private static string Base64Url(byte[] value) => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
