namespace UberPopug.Infrastructure.Auth.Models.API.Configuration
{
   public class IdentityConfiguration
   {
      public string PasswordSecret { get; set; }

      public int PasswordIterations { get; set; }

      public int PasswordHashSize { get; set; }

      public int RefreshTokenSize { get; set; }

      /// <summary>
      /// Requires 16 symbols
      /// </summary>
      public string JwtSecret { get; set; }

      public string Issuer { get; set; }

      public string Audience { get; set; }

      public string[] Roles { get; set; }

      public int JwtTokenLifetimeInMinutes { get; set; }

      public IdentityValidationParameters? IdentityValidationParameters { get; set; }

      public AuthorizationConstraints? AuthorizationConstraints { get; set; }

      public EmailSettings? EmailSettings { get; set; }

      public static IdentityConfiguration CreateDefault()
      {
         return new IdentityConfiguration
         {
            Audience = "authuser",
            Issuer = "authuser",
            JwtTokenLifetimeInMinutes = 60,
            PasswordHashSize = 32,
            PasswordIterations = 64,
            RefreshTokenSize = 8,
            IdentityValidationParameters = new IdentityValidationParameters
            {
               DigitRequired = true,
               LowerCaseLetterRequired = false,
               UpperCaseLetterRequired = true,
               MaxPasswordLength = 64,
               SpecialCharacterRequired = true,
               ValidateEmail = true
            },
            AuthorizationConstraints = new AuthorizationConstraints
            { 
               SettingsEnabled = false,
            }
         };
      }
   }
}
