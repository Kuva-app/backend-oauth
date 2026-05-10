namespace Kuva.Auth.Entities.Entities;

public sealed class RolePermission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
}
