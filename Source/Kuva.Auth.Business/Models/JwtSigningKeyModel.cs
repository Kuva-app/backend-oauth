using System.Security.Cryptography;

namespace Kuva.Auth.Business.Models;

public sealed record JwtSigningKeyModel(string KeyId, RSA Rsa);
