﻿using Microsoft.EntityFrameworkCore;
using UberPopug.Infrastructure.Auth.DataAccess.API;
using UberPopug.Infrastructure.Auth.DataAccess.Entities;
using UberPopug.Infrastructure.Auth.Models.API.Stores;

namespace UberPopug.Infrastructure.Auth.DataAccess.Stores
{
   public class ConfirmationTokenStore<TUser> : IConfirmationTokenStore
      where TUser : User
   {
      private readonly IAuthDatabaseContext<TUser> dbContext;

      public ConfirmationTokenStore(IAuthDatabaseContext<TUser> dbContext)
      {
         this.dbContext = dbContext;
      }

      public async Task AddConfirmationToken(Guid userId, string token, int expirationInHours)
      {
         var addingToken = new ConfirmationToken
         {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpirationDateUTC = DateTime.UtcNow.AddHours(expirationInHours)
         };

         await dbContext.ConfirmationTokens.AddAsync(addingToken);

         await dbContext.SaveChangesAsync();
      }

      public async Task RemoveExistingTokensForUser(Guid userId)
      {
         var tokensToRemove = await dbContext.ConfirmationTokens
            .Where(ct => ct.UserId == userId)
            .ToListAsync();

         if (tokensToRemove.Any())
         {
            dbContext.ConfirmationTokens.RemoveRange(tokensToRemove);

            await dbContext.SaveChangesAsync();
         }
      }

      public async Task<bool> TryFindAndRemoveToken(string token)
      {
         var existingToken = await dbContext.ConfirmationTokens.FirstOrDefaultAsync(t => t.Token == token);

         if (existingToken == null) return false;

         dbContext.ConfirmationTokens.Remove(existingToken);

         return true;
      }
   }
}
