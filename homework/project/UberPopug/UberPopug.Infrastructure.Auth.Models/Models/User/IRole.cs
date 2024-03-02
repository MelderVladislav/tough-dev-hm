using System;

namespace UberPopug.Infrastructure.Auth.Models.Models.User
{
   public interface IRole
   {
      Guid Id { get; set; }

      string Name { get; set; }
   }
}
