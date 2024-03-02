using UberPopug.Infrastructure.Auth.Models.Models.Token;

namespace UberPopug.Infrastructure.AuthModels.Services
{
   internal interface IJwtFactory
   {
      Task<AccessToken> GenerateEncodedToken(Guid id, string[] roles, string userLogin, string language);
   }
}
