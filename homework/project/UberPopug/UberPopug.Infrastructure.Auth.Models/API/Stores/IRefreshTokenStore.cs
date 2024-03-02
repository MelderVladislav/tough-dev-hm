using UberPopug.Infrastructure.Auth.Models.Models.User;

namespace UberPopug.Infrastructure.Auth.Models.API.Stores
{
   public interface IRefreshTokenStore
   {
      Task AddRefreshToken(string token, Guid userId, string userAgent, string userIP);

      Task<bool> CheckRefreshToken(string token, Guid userId);

      Task RemoveRefreshToken(string token);

      Task RemoveRefreshToken(Guid tokenId);

      Task<IEnumerable<IRefreshToken>> GetAllRefreshTokens(Guid userId);
   }
}
