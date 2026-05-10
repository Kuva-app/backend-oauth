using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record RegisterConsumerRequest(
    [property: Required, MaxLength(160)] string Name,
    [property: Required, EmailAddress, MaxLength(255)] string Email,
    [property: MaxLength(32)] string? Phone,
    [property: Required, MinLength(1)] string Password,
    [property: Required, MaxLength(32)] string AcceptedTermsVersion,
    [property: Required, MaxLength(32)] string AcceptedPrivacyPolicyVersion,
    bool PhotoProcessingConsent);
