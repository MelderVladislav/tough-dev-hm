using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Auth.API;
using UberPopug.Domains.Auth.Entities;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Auth;

public interface IAuthDatabaseContext<TUser> where TUser : User
{
    DbSet<TUser> Users { get; set; }

    DbSet<RefreshToken> RefreshTokens { get; set; }

    DbSet<Role> Roles { get; set; }

    DbSet<UserRole> UserRoles { get; set; }

    DbSet<ConfirmationToken> ConfirmationTokens { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}