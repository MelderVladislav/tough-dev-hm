using UberPopug.Infrastructure.Auth.Models.API.Configuration;
using UberPopug.Infrastructure.Auth.Models.API.Stores;

namespace UberPopug.Infrastructure.Auth.Models.Services.Identity
{
   internal class IdentityConfirmationService
   {
      public readonly EmailService emailService;
      private readonly IConfirmationTokenStore confirmationTokenStore;
      private readonly EmailSettings emailSettings;

      public IdentityConfirmationService(EmailService emailService,
         IConfirmationTokenStore confirmationTokenStore,
         IdentityConfiguration identityConfiguration)
      {
         this.emailService = emailService;
         this.emailSettings = identityConfiguration.EmailSettings;
         this.confirmationTokenStore = confirmationTokenStore;
      }

      public async Task SendConfirmationEmail(Guid userId, string userEmail, string subject)
      {
         var userToken = GenerateToken(32);

         await confirmationTokenStore.RemoveExistingTokensForUser(userId);

         await confirmationTokenStore.AddConfirmationToken(userId, userToken, emailSettings.ExpirationInHours);

         var htmlBody = emailSettings.HtmlBody.Replace("@token", userToken);

         emailService.SendEmail(from: emailSettings.From,
            to: userEmail,
            host: emailSettings.Host,
            port: emailSettings.Port.Value,
            fromPassword: emailSettings.Password,
            subject: subject,
            htmlContent: htmlBody,
            enableSsl: true);
      }

      public async Task<bool> TryConfirmEmail(string token)
      {
         return await confirmationTokenStore.TryFindAndRemoveToken(token);
      }

      private string GenerateToken(int length)
      {
         string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToLower();

         return new string(Enumerable.Repeat(chars, length)
               .Select(s => s[new Random().Next(s.Length)]).ToArray());
      }
   }
}
