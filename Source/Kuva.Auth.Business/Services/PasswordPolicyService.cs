using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.ValueObjects;
using Microsoft.Extensions.Options;

namespace Kuva.Auth.Business.Services;

public sealed class PasswordPolicyService(IOptions<PasswordPolicyOptions> options) : IPasswordPolicyService
{
    private readonly PasswordPolicyOptions _options = options.Value;

    public PasswordPolicyResult Validate(string password)
    {
        var violations = new List<string>();
        if (string.IsNullOrEmpty(password) || password.Length < _options.MinimumLength)
        {
            violations.Add($"A senha deve ter pelo menos {_options.MinimumLength} caracteres.");
        }

        if (_options.RequireUppercase && !password.Any(char.IsUpper))
        {
            violations.Add("A senha deve conter letra maiúscula.");
        }

        if (_options.RequireLowercase && !password.Any(char.IsLower))
        {
            violations.Add("A senha deve conter letra minúscula.");
        }

        if (_options.RequireDigit && !password.Any(char.IsDigit))
        {
            violations.Add("A senha deve conter número.");
        }

        if (_options.RequireSpecialCharacter && !password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            violations.Add("A senha deve conter caractere especial.");
        }

        return violations.Count == 0 ? PasswordPolicyResult.Success() : PasswordPolicyResult.Failure(violations);
    }
}
