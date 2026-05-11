namespace Kuva.Auth.Entities.Dtos.Responses;

public sealed record RegisterConsumerResponse(Guid UserId, string Email, string Status);
