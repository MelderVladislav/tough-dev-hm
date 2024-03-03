using UberPopug.Domains.Auth.Models;
using UberPopug.Domains.Auth.Models.User;

namespace UberPopug.Domains.Auth.Services.Stores;

public interface IUserStore<T> where T : class, IUser
{
    Task<AuthServiceResult<T>> InsertUser(T user, string[] roles);

    Task UpdateUser(T user);

    Task<T> FindUserById(Guid id);

    Task<string[]> GetUserRoleNames(Guid userId);

    Task<T> FindUserByLogin(string login);
}