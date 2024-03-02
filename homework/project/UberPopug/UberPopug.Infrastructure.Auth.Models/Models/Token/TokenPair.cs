using UberPopug.Infrastructure.Auth.Models.Codesets;

namespace UberPopug.Infrastructure.Auth.Models.Models.Token
{
   public class TokenPair
   {
      public string AccessToken { get; set; }

      public string RefreshToken { get; set; }

      public int ExpiresIn { get; set; }

      public AuthServiceError Error { get; set; }

      public TokenPair(string accessToken, string refreshToken, int expiresIn)
      {
         AccessToken = accessToken;
         RefreshToken = refreshToken;
         ExpiresIn = expiresIn;
      }

      public TokenPair(AuthServiceError error)
      { 
         Error = error;
      }

      public TokenPair(AuthErrorCode error)
      {
         Error = new AuthServiceError(error);
      }
   }
}
