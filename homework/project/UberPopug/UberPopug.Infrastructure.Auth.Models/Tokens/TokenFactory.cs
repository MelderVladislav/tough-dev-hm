using System.Security.Cryptography;
using UberPopug.Infrastructure.AuthModels.Services;

namespace UberPopug.Infrastructure.Auth.Models.Services.Tokens
{
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
}
