namespace Kuva.Auth.Business.Interfaces;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
