using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories
{
    public class UserRepo:IUserRepo
    {
        #region Prop
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        #endregion

        #region Ctor
        public UserRepo(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
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
