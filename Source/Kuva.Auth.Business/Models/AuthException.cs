namespace Kuva.Auth.Business.Models;

public sealed class AuthException(string code, string message, int statusCode = 400, IReadOnlyCollection<string>? details = null)
    : Exception(message)
{
    public string Code { get; } = code;
    public int StatusCode { get; } = statusCode;
    public IReadOnlyCollection<string>? Details { get; } = details;

    public static AuthException Unauthorized() => new("invalid_credentials", "E-mail ou senha inválidos.", 401);
    public static AuthException Forbidden(string message) => new("forbidden", message, 403);
    public static AuthException Conflict(string message) => new("conflict", message, 409);
    public static AuthException Unprocessable(string code, string message, IReadOnlyCollection<string>? details = null) => new(code, message, 422, details);
}
