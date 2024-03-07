using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.RoleVM;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories
{
    public class RoleRepo: IRoleRepo
    {
        #region prop
        private readonly RoleManager<IdentityRole> roleManager;

        #endregion

        #region Ctor
        public RoleRepo(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }


        #endregion

        #region method
        public async Task<bool> Create(CreateRoleVM roleVM)
        {
            try
            {
                var role = await roleManager.FindByNameAsync(roleVM.Name);
                if (role is not { })
                {
                    var result = await roleManager.CreateAsync(new IdentityRole() { Name =roleVM.Name });
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return false;

        }
        #endregion
    }
}
