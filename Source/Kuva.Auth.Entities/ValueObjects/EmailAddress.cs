using System.Text.RegularExpressions;

namespace Kuva.Auth.Entities.ValueObjects;

public sealed record EmailAddress
{
    private static readonly Regex EmailRegex = new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private EmailAddress(string value)
    {
        Value = value;
        NormalizedValue = Normalize(value);
    }

    public string Value { get; }
    public string NormalizedValue { get; }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !EmailRegex.IsMatch(value.Trim()))
        {
            throw new ArgumentException("E-mail inválido.", nameof(value));
        }

        return new EmailAddress(value.Trim());
    }

    public static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
