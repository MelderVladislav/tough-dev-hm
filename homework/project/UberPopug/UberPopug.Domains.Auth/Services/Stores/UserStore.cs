using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Auth.API;
using UberPopug.Domains.Auth.Entities;
using UberPopug.Domains.Auth.Models;
using UberPopug.Domains.Auth.Services.Codesets;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Auth.Services.Stores;

public class UserStore<TUser> : IUserStore<TUser>
    where TUser : User
{
    private readonly IAuthDatabaseContext<TUser> dbContext;

    public UserStore(IAuthDatabaseContext<TUser> dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<TUser> FindUserById(Guid id)
    {
        return await dbContext.Users.FindAsync(id);
    }

    public async Task<TUser> FindUserByLogin(string login)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<AuthServiceResult<TUser>> InsertUser(TUser user, string[] roles)
    {
        var userAlreadyCreated = dbContext.Users.Any(u => u.Login == user.Login || u.Email == user.Email);

        if (userAlreadyCreated) return new AuthServiceResult<TUser>(AuthErrorCode.ExistingIdentity);

        var dbRoles = dbContext.Roles.Where(dbRole => roles.Contains(dbRole.Name)).ToList();

        user.UserRoles = dbRoles
            .Select(role => new UserRole { Id = Guid.NewGuid(), UserId = user.Id, RoleId = role.Id }).ToList();

        await dbContext.Users.AddAsync(user);

        await dbContext.SaveChangesAsync();

        return new AuthServiceResult<TUser>(user);
    }

    public async Task<string[]> GetUserRoleNames(Guid userId)
    {
        return await dbContext
            .UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToArrayAsync();
    }

    public async Task UpdateUser(TUser user)
    {
        dbContext.Users.Update(user);

        await dbContext.SaveChangesAsync();
    }
}