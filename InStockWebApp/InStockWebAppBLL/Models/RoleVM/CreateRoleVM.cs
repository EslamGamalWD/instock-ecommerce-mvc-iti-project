using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.RoleVM
{
    public class CreateRoleVM
    {
        public string Name { get; set; }
        public string? Message { get; set; } = null;
    }
}
