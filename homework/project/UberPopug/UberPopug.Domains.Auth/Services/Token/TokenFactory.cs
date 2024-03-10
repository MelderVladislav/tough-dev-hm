using System.Security.Cryptography;

namespace UberPopug.Domains.Auth.Services.Tokens;

internal sealed class TokenFactory : ITokenFactory
{
    public string GenerateToken(int size)
    {
        var randomNumber = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}