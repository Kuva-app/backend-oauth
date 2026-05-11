namespace Kuva.Auth.Business.Interfaces;

public interface ITokenHashProvider
{
    string GenerateToken(int bytes);
    string HashToken(string token);
}
