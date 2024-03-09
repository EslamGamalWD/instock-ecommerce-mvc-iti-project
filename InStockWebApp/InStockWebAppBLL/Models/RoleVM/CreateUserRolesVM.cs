using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.RoleVM
{
    public class CreateUserRolesVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public List<UserInRoleVM> UserInRoleVM { get; set; }
    }
}
