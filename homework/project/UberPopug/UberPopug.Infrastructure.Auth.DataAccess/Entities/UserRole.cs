namespace UberPopug.Infrastructure.Auth.DataAccess.Entities
{
   public class UserRole
   {
      public Guid Id { get; set; }

      public int RoleId { get; set; }

      public Guid UserId { get; set; }

      public Role Role { get; set; }
   }
}
