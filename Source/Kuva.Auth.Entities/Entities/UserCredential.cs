namespace Kuva.Auth.Entities.Entities;

public sealed class UserCredential
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordAlgorithm { get; set; } = "MicrosoftPasswordHasherV3";
    public DateTimeOffset PasswordUpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public User? User { get; set; }
}
