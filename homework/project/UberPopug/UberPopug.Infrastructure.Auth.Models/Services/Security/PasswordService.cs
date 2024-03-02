using UberPopug.Infrastructure.Auth.Models.API.Configuration;
using UberPopug.Infrastructure.Auth.Models.Codesets;
using UberPopug.Infrastructure.Auth.Models.Models;
using UberPopug.Infrastructure.Auth.Models.Models.User;
using UberPopug.Infrastructure.Auth.Models.Utility.Hashing;

namespace UberPopug.Infrastructure.Auth.Models.Services.Security
{
   public class PasswordService: IPasswordService
   {
      private readonly IdentityConfiguration identityConfiguration;
      private readonly AuthorizationConstraints authorizationConstraints;

      public PasswordService(
         IdentityConfiguration identityConfiguration,
         AuthorizationConstraints authorizationConstraints)
      {
         this.authorizationConstraints = authorizationConstraints;
         this.identityConfiguration = identityConfiguration;
      }

      public string CreateHashFromPassword(Guid userId, string password)
      {
         var additionalSalt = userId
            .ToString()
            .ToLower()
            .Substring(0, 10);

         var passwordHash = HashHelper.GetPBKDF2Hash(
            password,
            identityConfiguration.PasswordSecret + additionalSalt,
            identityConfiguration.PasswordIterations,
            identityConfiguration.PasswordHashSize);

         return passwordHash;
      }

      public AuthServiceError HandleUserLoginAttempt<T>(string password, T user) where T: IUser
      {
         var canLogin = HandleLoginAttempt(user);

         if (!canLogin)
         {
            return new AuthServiceError(AuthErrorCode.UserIsBlocked);
         }

         var isValid = ValidatePassword(password, user.PasswordHash, user.Id);

         if (!isValid)
         {
            HandleFailedAttempts(user);

            return new AuthServiceError(AuthErrorCode.WrongPassword);
         }

         return null;
      }

      private  bool ValidatePassword(string password, string originalPasswordHash, Guid userId)
      {
         var passwordHash = CreateHashFromPassword(userId, password);

         var isEqual = string.Equals(passwordHash, originalPasswordHash, StringComparison.InvariantCultureIgnoreCase);

         return isEqual;
      }

      private bool HandleLoginAttempt<T>(T user) where T : IUser // TODO: handle email activation
      {
         if (!authorizationConstraints.SettingsEnabled)
         {
            return true;
         }

         if (user.IsActive)
         {
            return true;
         }

         if (DateTime.UtcNow < user.BlockedUntilDateUtc)
         {
            return false;
         }

         user.IsActive = true;
         user.BlockedUntilDateUtc = null;

         return true;

      }

      private void HandleFailedAttempts<T>(T user) where T : IUser
      {
         if (!authorizationConstraints.SettingsEnabled)
         {
            return;
         }

         if (user.AttemptsToLogin == authorizationConstraints.AttemptsToLogin)
         {
            user.IsActive = false;
            user.BlockedUntilDateUtc = DateTime.UtcNow.AddMilliseconds(authorizationConstraints.BlockTimespanInMs);
            user.AttemptsToLogin = 0;
         }
         else
         {
            user.AttemptsToLogin++;
         }

         return;
      }
   }
}
