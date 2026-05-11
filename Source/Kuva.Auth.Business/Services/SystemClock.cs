using Kuva.Auth.Business.Interfaces;

namespace Kuva.Auth.Business.Services;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
