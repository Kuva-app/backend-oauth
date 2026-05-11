using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Entities.Entities;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Active;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public UserCredential? Credential { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<StoreOperator> StoreOperators { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<UserConsent> UserConsents { get; set; } = [];
    public ICollection<TermsAcceptance> TermsAcceptances { get; set; } = [];
}
