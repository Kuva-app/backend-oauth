using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record RegisterConsumerRequest(
    [param: Required, MaxLength(160)] string Name,
    [param: Required, EmailAddress, MaxLength(255)] string Email,
    [param: MaxLength(32)] string? Phone,
    [param: Required, MinLength(1)] string Password,
    [param: Required, MaxLength(32)] string AcceptedTermsVersion,
    [param: Required, MaxLength(32)] string AcceptedPrivacyPolicyVersion,
    [param: Required] bool PhotoProcessingConsent);
