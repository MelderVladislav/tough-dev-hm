using Microsoft.EntityFrameworkCore;
using UberPopug.Infrastructure.Auth.DataAccess.Entities;

namespace UberPopug.Infrastructure.Auth.DataAccess.API
{
   public abstract class AuthContextBase<TUser> : DbContext, IAuthDatabaseContext<TUser>
      where TUser : User
   {
      public AuthContextBase(DbContextOptions options) : base(options)
      {
      }

      public AuthContextBase() 
      {
      }

      public DbSet<TUser> Users { get; set; }

      public DbSet<RefreshToken> RefreshTokens { get; set; }

      public DbSet<Role> Roles { get; set; }

      public DbSet<UserRole> UserRoles { get; set; }

      public DbSet<ConfirmationToken> ConfirmationTokens { get; set; }

      public abstract IEnumerable<string> GetRoleNames();
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         this.AddAuthEntities(modelBuilder, GetRoleNames());
      }
   }
}
