using UberPopug.Infrastructure.Auth.Models.Models;
using UberPopug.Infrastructure.Auth.Models.Models.Token;
using UberPopug.Infrastructure.Auth.Models.Models.User;

namespace UberPopug.Infrastructure.Auth.Models.API.Services
{
   public interface IIdentityService<T>
      where T : class, IUser, new()
   {
      Task<AuthServiceResult<T>> AddUser(string login, string email, string password, string[] roles, string language);

      Task<AuthServiceResult<TokenPair>> LoginUser(string login, string password, string userAgent, string userIP);

      Task<AuthServiceResult<TokenPair>> UpdateRefreshToken(string refreshToken, string accessToken, string userAgent, string userIP);

      Task SendConfirmationMail(Guid userId);

      Task<bool> ConfirmEmail(string confirmationToken);

      Task<AuthServiceResult<bool>> TryActivateUser(string token);
   }
}
