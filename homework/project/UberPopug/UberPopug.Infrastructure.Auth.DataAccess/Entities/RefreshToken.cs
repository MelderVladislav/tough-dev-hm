using UberPopug.Infrastructure.Auth.Models.Models.Token;
using UberPopug.Infrastructure.Auth.Models.Models.User;

namespace UberPopug.Infrastructure.Auth.DataAccess.Entities
{
   public class RefreshToken: IRefreshToken
   {
      public Guid Id { get; set; }

      public string Token { get; set; }

      public string? UserAgent { get; set; }

      public string? UserIP { get; set; }

      public DateTime CreationDateUTC { get; set; }

      public Guid UserId { get; set; }
   }
}
