using UberPopug.Domains.Auth.Models.Token;

namespace UberPopug.Domains.Auth.Services.Users;

public interface IUserService
{
    Task<TokenPair> Refresh(string accessToken, string oldToken);

    Task<Guid?> Register(string login, string email, string password, string role);

    Task<TokenPair> Login(string login, string password);
}