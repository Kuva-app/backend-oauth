namespace Kuva.Auth.Entities.Entities;

public sealed class UserRole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public User? User { get; set; }
    public Role? Role { get; set; }
}
