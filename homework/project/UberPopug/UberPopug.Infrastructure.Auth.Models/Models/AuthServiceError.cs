using UberPopug.Infrastructure.Auth.Models.Codesets;
using UberPopug.Infrastructure.Auth.Models.Utility.Enums;

namespace UberPopug.Infrastructure.Auth.Models.Models
{
   public class AuthServiceError
   {
      public AuthErrorCode ErrorCode { get; set; }

      public string? ErrorMessage { get; set; }

      public string? AdditionalMessage { get; set; }

      public AuthServiceError(AuthErrorCode error, string? additionalMessage = null)
      {
         ErrorCode = error;
         ErrorMessage = error.GetDescription();
         AdditionalMessage = additionalMessage;
      }
   }
}
