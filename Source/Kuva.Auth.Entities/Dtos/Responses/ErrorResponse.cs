namespace Kuva.Auth.Entities.Dtos.Responses;

public sealed record ErrorResponse(string Code, string Message, IReadOnlyCollection<string>? Details = null);
