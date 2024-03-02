namespace UberPopug.Infrastructure.Auth.Models.API.Configuration
{
   public class IdentityValidationParameters
   {
      public bool DigitRequired { get; set; }

      public bool UpperCaseLetterRequired { get; set; }

      public bool LowerCaseLetterRequired { get; set; }

      public bool SpecialCharacterRequired { get; set; }

      public bool ValidateEmail { get; set; }

      public int? MinPasswordLenght { get; set; }

      public int? MaxPasswordLength { get; set; }
   }
}
