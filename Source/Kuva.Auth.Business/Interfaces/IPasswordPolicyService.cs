using Kuva.Auth.Entities.ValueObjects;

namespace Kuva.Auth.Business.Interfaces;

public interface IPasswordPolicyService
{
    PasswordPolicyResult Validate(string password);
}
