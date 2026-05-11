using System.Security.Cryptography;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Microsoft.Extensions.Options;

namespace Kuva.Auth.Business.Providers;

public sealed class InMemoryJwtKeyProvider : IJwtKeyProvider, IDisposable
{
    private readonly JwtOptions _options;
    private readonly RSA _rsa;

    public InMemoryJwtKeyProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        _rsa = RSA.Create(2048);

        if (!string.IsNullOrWhiteSpace(_options.PrivateKeyPem))
        {
            _rsa.ImportFromPem(_options.PrivateKeyPem);
        }
    }

    public JwtSigningKeyModel GetSigningKey() => new(_options.KeyId, _rsa);

    public void Dispose() => _rsa.Dispose();
}
