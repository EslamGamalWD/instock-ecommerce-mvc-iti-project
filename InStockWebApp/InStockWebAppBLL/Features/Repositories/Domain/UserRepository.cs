using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class UserRepository:IUserRepository
    {
        #region Prop
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        #endregion

        #region Ctor
        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        #endregion

        #region Method
        public async Task< bool> Create(CreateUserVM createUserVM)
        {
            //var user = mapper.Map<User>(createUserVM);
            //var result1 = await userManager.AddToRoleAsync(user, role.Name);
            return true;
        }
        #endregion

    }
}
