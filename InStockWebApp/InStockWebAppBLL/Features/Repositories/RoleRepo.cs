using AutoMapper;
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
        private readonly IMapper mapper;

        #endregion

        #region Ctor
        public RoleRepo(RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper=mapper;
        }


        #endregion

        #region method
        public async Task<bool> Create(CreateRoleVM roleVM)
        {
            try
            {
                var getRoleByName = await roleManager.FindByNameAsync(roleVM.Name);
                if (getRoleByName is not { })
                {
                    var role = mapper.Map<IdentityRole>(roleVM);
                    var result = await roleManager.CreateAsync(role);
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return false;

        }

        public async Task<IEnumerable<GetAllRoleVM>> GetAll()
        {
            var roles =  roleManager.Roles.ToList();
            var rolesVM = mapper.Map<IEnumerable<GetAllRoleVM>>(roles);
            return rolesVM;
        }
        #endregion
    }
}
