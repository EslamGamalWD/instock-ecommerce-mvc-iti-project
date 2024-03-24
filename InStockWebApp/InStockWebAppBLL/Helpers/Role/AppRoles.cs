using InStockWebAppDAL.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Helpers.Role
{
    public static class AppRoles
    {
        public const string Admin = nameof(UserType.Admin);
        public const string Customer = nameof(UserType.Customer);
        
        public static string EnumToString(Enum value)
        {
            var customrole = value.ToString();
            return customrole;
        }
    }
}
