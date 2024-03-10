using UberPopug.Domains.Auth.Models.Token;

namespace UberPopug.Domains.Auth.Services.Tokens;

internal interface IJwtFactory
{
    Task<AccessToken> GenerateEncodedToken(Guid id, string[] roles, string userLogin, string language);
}