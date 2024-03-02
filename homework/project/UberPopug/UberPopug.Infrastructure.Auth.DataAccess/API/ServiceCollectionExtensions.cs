using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UberPopug.Infrastructure.Auth.DataAccess.Entities;
using UberPopug.Infrastructure.Auth.DataAccess.Stores;
using UberPopug.Infrastructure.Auth.Models.API.Configuration;
using UberPopug.Infrastructure.Auth.Models.API.Extensions;

namespace UberPopug.Infrastructure.Auth.DataAccess.API
{
   public static class ServiceCollectionExtensions
   {
       public static IServiceCollection SetupAuthContext<UserDbContext, TUser>(this WebApplicationBuilder builder)
           where UserDbContext : DbContext, IAuthDatabaseContext<TUser>
           where TUser : User, new()
       {
           var identityConfiguration = IdentityConfiguration.CreateDefault();
           identityConfiguration.JwtSecret = EnsureConfigurationField("IdentityConfiguration:JwtSecret", builder.Configuration);
           identityConfiguration.PasswordSecret = EnsureConfigurationField("IdentityConfiguration:PasswordSecret", builder.Configuration);
         
           builder.Configuration.GetSection("IdentityConfiguration").Bind(identityConfiguration);

           var dbConnectionString = EnsureConfigurationField("ConnectionStrings:ConfigurationDatabase", builder.Configuration);
           builder.Services.AddDbContext<UserDbContext>(opt => opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
         
           return builder.Services.AddAuthServices<UserDbContext, TUser>(identityConfiguration);
       }

       public static IServiceCollection AddAuthServices<UserDbContext, TUser>(this IServiceCollection services,
           IdentityConfiguration identityConfiguration)
           where UserDbContext : DbContext, IAuthDatabaseContext<TUser>
           where TUser : User, new()
       {
           return services
               .AddTransient<IAuthDatabaseContext<TUser>, UserDbContext>()
               .AddIdentityServices<TUser, UserStore<TUser>, RefreshTokensStore<TUser>, ConfirmationTokenStore<TUser>>(identityConfiguration);
       }

       private static string EnsureConfigurationField(string fieldName, IConfiguration configuration)
       {
           var value = configuration[fieldName];

           if (value == null)
           {
               throw new ArgumentNullException($"Предоставьте поле {fieldName} в конфигурации приложения");
           }

           return value;
       }
       
       
      public static void AddAuthEntities<TUser>(this IAuthDatabaseContext<TUser> dbContext, ModelBuilder modelBuilder, IEnumerable<string> roles)
         where TUser: User
      {
         modelBuilder
               .Entity<TUser>()
               .HasMany(u => u.RefreshTokens)
               .WithOne();

         modelBuilder
               .Entity<TUser>()
               .HasMany(u => u.UserRoles)
               .WithOne()
               .HasForeignKey(ur => ur.UserId);

         modelBuilder
               .Entity<TUser>()
               .HasIndex(u => u.Login)
               .IsUnique();

         modelBuilder
               .Entity<TUser>()
               .HasIndex(u => u.Email)
               .IsUnique();

         modelBuilder
               .Entity<TUser>()
               .Property(u => u.CreationDataUTC)
               .HasDefaultValue(DateTime.UtcNow);

         modelBuilder
               .Entity<TUser>()
               .HasMany(u => u.RefreshTokens)
               .WithOne()
               .HasForeignKey(r => r.UserId);

         modelBuilder
               .Entity<RefreshToken>()
               .Property(rt => rt.CreationDateUTC)
               .HasDefaultValue(DateTime.UtcNow);

         modelBuilder
            .Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

         modelBuilder
            .Entity<Role>()
            .HasMany(c => c.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.RoleId);

         modelBuilder
            .Entity<Role>()
            .HasData(roles.Select((r, index) => new Role { Id = index + 1, Name = r }));

         modelBuilder
            .Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

         modelBuilder
            .Entity<ConfirmationToken>()
            .Property(p => p.CreationDateUTC)
            .HasDefaultValue(DateTime.UtcNow);

         modelBuilder
            .Entity<ConfirmationToken>()
            .Property(p => p.CreationDateUTC)
            .HasDefaultValue(DateTime.UtcNow);

         modelBuilder
               .Entity<TUser>()
               .HasMany(u => u.ConfirmationTokens)
               .WithOne()
               .HasForeignKey(r => r.UserId);

         //modelBuilder
         //      .Entity<User>()
         //      .HasMany(p => p.Roles)
         //      .WithMany()
         //      .UsingEntity<UserRole>(pt => pt.HasOne(p => p.Role)
         //                                    .WithMany(p => p.UserRoles)
         //                                    .HasForeignKey(p => p.RoleId),
         //                              pt => pt.HasOne(p => p.User)
         //                              .WithMany(p => p.UserRoles)
         //                              .HasForeignKey(p => p.UserId));

         //modelBuilder
         //   .Entity<UserRole>()
         //   .HasOne(c => c.User)
         //   .WithMany(c => c.UserRoles);

         //modelBuilder
         //   .Entity<UserRole>()
         //   .HasOne(c => c.Role)
         //   .WithMany(c => c.UserRoles);

      }
   }
}
