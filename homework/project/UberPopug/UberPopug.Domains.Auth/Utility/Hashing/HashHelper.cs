using System.Security.Cryptography;
using System.Text;

namespace UberPopug.Domains.Auth.Utility.Hashing;

public static class HashHelper
{
    public static string GetPBKDF2Hash(string input, string salt, int iterations, int hashSize)
    {
        var saltBytes = Encoding.ASCII.GetBytes(salt);
        var pbkdf2 = new Rfc2898DeriveBytes(input, saltBytes, iterations);
        var hash = pbkdf2.GetBytes(hashSize);
        var base64String = Convert.ToBase64String(hash);

        return base64String;
    }
}