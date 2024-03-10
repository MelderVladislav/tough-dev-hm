namespace UberPopug.Domains.Auth.Services.Stores;

public interface IConfirmationTokenStore
{
    Task AddConfirmationToken(Guid userId, string token, int expirationInHours);

    Task RemoveExistingTokensForUser(Guid userId);

    Task<bool> TryFindAndRemoveToken(string token);
}