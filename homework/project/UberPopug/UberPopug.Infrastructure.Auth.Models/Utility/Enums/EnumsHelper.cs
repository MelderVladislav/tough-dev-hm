using System;
using System.ComponentModel;

namespace UberPopug.Infrastructure.Auth.Models.Utility.Enums
{
   public static class EnumsHelper
   {
      public static string GetDescription(this Enum enumVal)
      {
         var type = enumVal.GetType();
         var memInfo = type.GetMember(enumVal.ToString());
         var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

         return attributes.Length > 0 ? (attributes[0] as DescriptionAttribute)?.Description : null;
      }
   }
}
