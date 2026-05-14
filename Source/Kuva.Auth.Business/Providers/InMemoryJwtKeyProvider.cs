using System.Security.Cryptography;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Microsoft.Extensions.Options;

namespace Kuva.Auth.Business.Providers;

public sealed class InMemoryJwtKeyProvider : IJwtKeyProvider, IDisposable
{
    private readonly JwtOptions _options;
    private readonly ECDsa _ecdsa;

    public InMemoryJwtKeyProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        _ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

        if (!string.IsNullOrWhiteSpace(_options.PrivateKeyPem))
        {
            _ecdsa.ImportFromPem(_options.PrivateKeyPem);
        }
    }

    public JwtSigningKeyModel GetSigningKey() => new(_options.KeyId, _ecdsa);

    public void Dispose() => _ecdsa.Dispose();
}