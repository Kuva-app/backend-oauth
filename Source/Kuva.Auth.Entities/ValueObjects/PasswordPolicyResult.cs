namespace Kuva.Auth.Entities.ValueObjects;

public sealed record PasswordPolicyResult(bool IsValid, IReadOnlyCollection<string> Violations)
{
    public static PasswordPolicyResult Success() => new(true, Array.Empty<string>());
    public static PasswordPolicyResult Failure(IReadOnlyCollection<string> violations) => new(false, violations);
}
