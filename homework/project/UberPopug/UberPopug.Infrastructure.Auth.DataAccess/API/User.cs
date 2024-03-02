using UberPopug.Infrastructure.Auth.DataAccess.Entities;
using UberPopug.Infrastructure.Auth.Models.Models.User;

namespace UberPopug.Infrastructure.Auth.DataAccess.API
{
   public class User : IUser
   {
      public Guid Id { get; set; }

      public bool IsActive { get; set; }

      public bool IsEmailActivated { get; set; }

      public int AttemptsToLogin { get; set; }

      public DateTime? BlockedUntilDateUtc { get; set; }

      public string Login { get; set; }

      public string Email { get; set; }

      public string? PasswordHash { get; set; }
      
      public string DefaultLanguage { get; set; }

      public DateTime CreationDataUTC { get; set; }

      public List<UserRole> UserRoles { get; set; }

      public List<RefreshToken> RefreshTokens { get; set; }

      public List<ConfirmationToken> ConfirmationTokens { get; set; }
   }
}
