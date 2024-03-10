using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Auth.API;
using UberPopug.Domains.Auth.Entities;
using UberPopug.Domains.Auth.Models.User;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Auth.Services.Stores;

public class RefreshTokensStore<TUser> : IRefreshTokenStore
    where TUser : User
{
    private readonly IAuthDatabaseContext<TUser> dbContext;

    public RefreshTokensStore(IAuthDatabaseContext<TUser> dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddRefreshToken(string token, Guid userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId
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