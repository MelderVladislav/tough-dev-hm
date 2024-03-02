using UberPopug.Infrastructure.Auth.Models.Models;
using UberPopug.Infrastructure.Auth.Models.Models.User;

namespace UberPopug.Infrastructure.Auth.Models.Services.Security
{
   public interface IPasswordService
   {
      string CreateHashFromPassword(Guid userId, string password);

      AuthServiceError HandleUserLoginAttempt<T>(string password, T user) where T : IUser;
   }
}
