using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UberPopug.Domains.Auth.Models.Token;

namespace UberPopug.Domains.Auth.Services.Security;

public interface ITokenService
{
    Task<TokenPair> GenerateTokenPair(Guid userId, string login, string[] roles, string language);

    (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
}