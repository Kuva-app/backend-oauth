using System.Text.Json.Serialization;

namespace Kuva.Auth.Entities.Dtos.Responses;

public sealed record JwksResponse(IReadOnlyCollection<JwksKeyResponse> Keys);

public sealed record JwksKeyResponse(
    [property: JsonPropertyName("kty")] string KeyType,
    [property: JsonPropertyName("use")] string Use,
    [property: JsonPropertyName("kid")] string KeyId,
    [property: JsonPropertyName("alg")] string Algorithm,
    [property: JsonPropertyName("n")] string Modulus,
    [property: JsonPropertyName("e")] string Exponent);
