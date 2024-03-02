namespace UberPopug.Infrastructure.Auth.Models.API.Configuration
{
   public class AuthorizationConstraints
   {
      public bool SettingsEnabled { get; set; }

      public int AttemptsToLogin { get; set; }

      public int BlockTimespanInMs { get; set; }
   }
}
