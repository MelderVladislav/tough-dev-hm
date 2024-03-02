using System;

namespace UberPopug.Infrastructure.Auth.Models.Models.User
{
   public interface IUserRole
   {
      Guid UserId { get; set; }

      Guid RoleId { get; set; }
   }
}
