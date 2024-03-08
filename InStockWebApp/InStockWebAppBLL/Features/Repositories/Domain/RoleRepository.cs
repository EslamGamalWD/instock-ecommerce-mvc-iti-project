using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.RoleVM;
using Microsoft.AspNetCore.Identity;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class RoleRepository: IRoleRepository
    {
        #region prop
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        #endregion

        #region Ctor
        public RoleRepository(RoleManager<IdentityRole> roleManager,IMapper mapper)
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
