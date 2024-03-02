using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UberPopug.Infrastructure.AuthModels.Services;
using UberPopug.Infrastructure.Auth.Models.API.Configuration;
using UberPopug.Infrastructure.Auth.Models.API.Services;
using UberPopug.Infrastructure.Auth.Models.API.Stores;
using UberPopug.Infrastructure.Auth.Models.Models.Token;
using UberPopug.Infrastructure.Auth.Models.Models.User;
using UberPopug.Infrastructure.Auth.Models.Services.Identity;
using UberPopug.Infrastructure.Auth.Models.Services.Security;
using UberPopug.Infrastructure.Auth.Models.Services.Tokens;

namespace UberPopug.Infrastructure.Auth.Models.API.Extensions
{
   public static class ServiceCollectionExtensions
   {
      public static IServiceCollection AddIdentityServices<T, T2, T3, T4>(this IServiceCollection services, IdentityConfiguration configurationOptions) 
         where T: class, IUser, new()
         where T2 : class, IUserStore<T>
         where T3 : class, IRefreshTokenStore
         where T4 : class, IConfirmationTokenStore
      {
         ConfigureAuthorizationMiddleware(services, configurationOptions);

         services.AddScoped<IPasswordService, PasswordService>()
            .AddSingleton(configurationOptions.AuthorizationConstraints)
            .AddSingleton(configurationOptions)
            .AddScoped<IJwtTokenHandler, JwtTokenHandler>()
            .AddScoped<ITokenFactory, TokenFactory>()
            .AddScoped<IJwtFactory, JwtFactory>()
            .AddScoped<IUserStore<T>, T2>()
            .AddScoped<IRefreshTokenStore, T3>()
            .AddScoped<IConfirmationTokenStore, T4>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IdentityValidator>()
            .AddScoped<IdentityConfirmationService>()
            .AddScoped<EmailService>()
            .AddScoped<IIdentityService<T>, IdentityService<T>>();

         return services;
      }

      private static void ConfigureAuthorizationMiddleware(IServiceCollection services, IdentityConfiguration configurationOptions)
      {
         var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configurationOptions.JwtSecret));

         // Configure JwtIssuerOptions
         services.Configure<JwtIssuerOptions>(options =>
         {
            options.Issuer = configurationOptions.Issuer;
            options.Audience = configurationOptions.Audience;
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
         });

         var tokenValidationParameters = new TokenValidationParameters
         {
            ValidateIssuer = true,
            ValidIssuer = configurationOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = configurationOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
         };
         
         services.AddAuthentication(options =>
         {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(configureOptions =>
         {
            configureOptions.ClaimsIssuer = configurationOptions.Issuer;
            configureOptions.TokenValidationParameters = tokenValidationParameters;
            configureOptions.SaveToken = true;
         });

         //// api user claim policy
         //services.AddAuthorization(options =>
         //{
         //   options.AddPolicy("ApiUser", policy => policy.RequireClaim("rol", "api_access"));
         //});
      }
   }
}
