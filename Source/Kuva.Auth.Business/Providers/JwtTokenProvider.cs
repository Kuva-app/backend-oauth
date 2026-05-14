using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kuva.Auth.Business.Providers;

public sealed class JwtTokenProvider(IJwtKeyProvider keyProvider, IClock clock, IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _options = options.Value;

    public JwtTokenResult CreateAccessToken(AuthenticatedUserModel user)
    {
        var now = clock.UtcNow;
        var expiresAt = now.AddMinutes(_options.AccessTokenMinutes);
        var signingKey = keyProvider.GetSigningKey();
        var credentials = new SigningCredentials(new ECDsaSecurityKey(signingKey.Ecdsa) { KeyId = signingKey.KeyId }, SecurityAlgorithms.EcdsaSha256);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(AuthClaimTypes.Roles, role)));
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(user.Permissions.Select(permission => new Claim(AuthClaimTypes.Permissions, permission)));

        if (user.StoreId is not null && user.Roles.Any(role => RoleNames.MerchantRoles.Contains(role)))
        {
            claims.Add(new Claim(AuthClaimTypes.StoreId, user.StoreId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new JwtTokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
