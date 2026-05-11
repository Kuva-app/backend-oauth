namespace Kuva.Auth.Entities.ValueObjects;

public sealed record TokenHash(string Value)
{
    public static TokenHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Hash de token obrigatório.", nameof(value));
        }

        return new TokenHash(value);
    }
}
