using UberPopug.Domains.Auth.Models.User;

namespace UberPopug.Domains.Auth.Services.Stores;

public interface IRefreshTokenStore
{
    Task AddRefreshToken(string token, Guid userId);

    Task<bool> CheckRefreshToken(string token, Guid userId);

    Task RemoveRefreshToken(string token);

    Task RemoveRefreshToken(Guid tokenId);

    Task<IEnumerable<IRefreshToken>> GetAllRefreshTokens(Guid userId);
}