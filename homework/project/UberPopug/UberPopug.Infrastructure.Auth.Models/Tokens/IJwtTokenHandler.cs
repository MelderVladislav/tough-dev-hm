using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace UberPopug.Infrastructure.Auth.Models.Services.Tokens
{
   internal interface IJwtTokenHandler
   {
      string WriteToken(JwtSecurityToken jwt);

      ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
   }
}
