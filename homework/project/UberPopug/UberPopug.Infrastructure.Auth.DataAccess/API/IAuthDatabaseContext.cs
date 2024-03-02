using Microsoft.EntityFrameworkCore;
using UberPopug.Infrastructure.Auth.DataAccess.Entities;

namespace UberPopug.Infrastructure.Auth.DataAccess.API
{
   public interface IAuthDatabaseContext<TUser> where TUser: User
   {
      DbSet<TUser> Users { get; set; }

      DbSet<RefreshToken> RefreshTokens { get; set; }

      DbSet<Role> Roles { get; set; }

      DbSet<UserRole> UserRoles { get; set; }

      DbSet<ConfirmationToken> ConfirmationTokens { get; set; }

      Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
   }
}
