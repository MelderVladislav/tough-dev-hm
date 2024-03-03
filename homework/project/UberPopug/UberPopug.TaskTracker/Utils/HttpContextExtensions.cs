namespace Toque.IdeaSpace.API.Utils
{
   public static class HttpContextExtensions
   {
      public static string GetJwtToken(this HttpContext context)
      {
         var success = context.Request.Headers.TryGetValue("Authorization", out var authToken);

         if (!success) return null;

         var accessToken = authToken.ToString().Replace("Bearer ", "");

         return accessToken;
      }

      public static (string userIP, string userAgent) GetUserAdditionalInfo(this HttpContext context)
      {
         return (context.Connection.RemoteIpAddress.ToString(), context.Request.Headers["User-Agent"].ToString());
      }
   }
}
