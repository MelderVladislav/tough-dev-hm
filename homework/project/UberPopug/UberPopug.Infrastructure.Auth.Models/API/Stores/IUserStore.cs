using UberPopug.Infrastructure.Auth.Models.Models;
using UberPopug.Infrastructure.Auth.Models.Models.User;

namespace UberPopug.Infrastructure.Auth.Models.API.Stores
{
   public interface IUserStore<T> where T : class, IUser
   {
      Task<AuthServiceResult<T>> InsertUser(T user, string[] roles);

      Task UpdateUser(T user);

      Task<T> FindUserById(Guid id);

      Task<string[]> GetUserRoleNames(Guid userId);

      Task<T> FindUserByLogin(string login);
   }
}
