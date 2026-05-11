using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Entities.Entities;

public sealed class StoreOperator
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StoreId { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; } = string.Empty;
    public StoreOperatorStatus Status { get; set; } = StoreOperatorStatus.Active;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public User? User { get; set; }
}
