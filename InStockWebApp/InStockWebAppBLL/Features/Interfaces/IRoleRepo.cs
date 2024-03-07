using InStockWebAppBLL.Models.RoleVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces
{
    public interface IRoleRepo
    {
        Task<bool> Create(CreateRoleVM roleVM);
        Task<IEnumerable<GetAllRoleVM>> GetAll();
    }
}
