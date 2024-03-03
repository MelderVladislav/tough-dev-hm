using UberPopug.Domains.Auth.Models;
using UberPopug.Domains.Auth.Models.User;

namespace UberPopug.Domains.Auth.Services.Security;

public interface IPasswordService
{
    string CreateHashFromPassword(Guid userId, string password);

    AuthServiceError HandleUserLoginAttempt<T>(string password, T user) where T : IUser;
}