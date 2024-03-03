using UberPopug.Domains.Auth.Models;
using UberPopug.Domains.Auth.Models.Token;
using UberPopug.Domains.Auth.Models.User;

namespace UberPopug.Domains.Auth.Services.Identity;

public interface IIdentityService<T>
    where T : class, IUser, new()
{
    Task<AuthServiceResult<T>> AddUser(string login, string email, string password, string[] roles);

    Task<AuthServiceResult<TokenPair>> LoginUser(string login, string password);

    Task<AuthServiceResult<TokenPair>> UpdateRefreshToken(string refreshToken, string accessToken);
}