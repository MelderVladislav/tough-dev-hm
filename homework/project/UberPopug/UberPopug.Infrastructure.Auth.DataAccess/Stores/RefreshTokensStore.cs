using Microsoft.EntityFrameworkCore;
using UberPopug.Infrastructure.Auth.DataAccess.Entities;
using UberPopug.Infrastructure.Auth.DataAccess.API;
using UberPopug.Infrastructure.Auth.Models.API.Stores;
using UberPopug.Infrastructure.Auth.Models.Models.User;

namespace UberPopug.Infrastructure.Auth.DataAccess.Stores
{
   public class RefreshTokensStore<TUser> : IRefreshTokenStore
      where TUser: User
   {
      private readonly IAuthDatabaseContext<TUser> dbContext;

      public RefreshTokensStore(IAuthDatabaseContext<TUser> dbContext)
      {
         this.dbContext = dbContext;
      }

      public async Task AddRefreshToken(string token, Guid userId, string userAgent, string userIP)
      {
         var refreshToken = new RefreshToken
         {
            Token = token,
            UserId = userId,
            UserAgent = userAgent,
            UserIP = userIP
         };

         await dbContext.RefreshTokens.AddAsync(refreshToken);

         await dbContext.SaveChangesAsync();
      }

      public async Task RemoveRefreshToken(string token)
      {
         var oldToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

         dbContext.RefreshTokens.Remove(oldToken);

         await dbContext.SaveChangesAsync();
      }

      public async Task RemoveRefreshToken(Guid tokenId)
      {
         var token = await dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Id == tokenId);

         dbContext.RefreshTokens.Remove(token);

         await dbContext.SaveChangesAsync();
      }

      public async Task<IEnumerable<IRefreshToken>> GetAllRefreshTokens(Guid userId)
      {
         var tokens = await dbContext.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();

         return tokens;
      }

      public async Task<bool> CheckRefreshToken(string token, Guid userId)
      {
         return await dbContext.RefreshTokens.AnyAsync(rt => rt.Token == token && rt.UserId == userId);
      }
   }
}
