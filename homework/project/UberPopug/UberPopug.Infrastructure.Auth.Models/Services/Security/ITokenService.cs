using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UberPopug.Infrastructure.Auth.Models.Models.Token;

namespace UberPopug.Infrastructure.Auth.Models.Services.Security
{
   public interface ITokenService
   {
      Task<TokenPair> GenerateTokenPair(Guid userId, string login, string[] roles, string language);

      (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
   }
}
