namespace UberPopug.Infrastructure.Auth.Models.API.Stores
{
   public interface IConfirmationTokenStore
   {
      Task AddConfirmationToken(Guid userId, string token, int expirationInHours);

      Task RemoveExistingTokensForUser(Guid userId);

      Task<bool> TryFindAndRemoveToken(string token);
   }
}
