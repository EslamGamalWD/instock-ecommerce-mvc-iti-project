using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.RoleVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class RoleRepository: IRoleRepository
    {
        #region prop
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext applicationDbContext;

        #endregion

        #region Ctor
        public RoleRepository(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IMapper mapper,ApplicationDbContext applicationDbContext)
        {
            this.roleManager = roleManager;
            this.userManager=userManager;
            this.mapper=mapper;
            this.applicationDbContext=applicationDbContext;
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

        public async Task<bool> CreateUsersRole(CreateUserRolesVM roleVM)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(roleVM.RoleId);

                for (int i = 0; i < roleVM.UserInRoleVM.Count; i++)
                {

                    var user = await userManager.FindByIdAsync(roleVM.UserInRoleVM[i].UserId);

                    IdentityResult result = null;

                    if (roleVM.UserInRoleVM[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {

                        result = await userManager.AddToRoleAsync(user, role.Name);

                    }
                    else if (!roleVM.UserInRoleVM[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }


                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public async Task<IEnumerable<GetAllRoleVM>> GetAll()
        {
            var roles =  roleManager.Roles.ToList();
            var rolesVM = mapper.Map<IEnumerable<GetAllRoleVM>>(roles);
            return rolesVM;
        }

        public async Task<CreateUserRolesVM> GetUsersRole(string roleId)
        {
            CreateUserRolesVM rolevm = new CreateUserRolesVM();
            rolevm.RoleId=roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            rolevm.RoleName=role.Name;

            rolevm.UserInRoleVM = new List<UserInRoleVM>();

            foreach (var user in await applicationDbContext.Users.ToListAsync())
            {
                var userInRole = new UserInRoleVM()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }

                rolevm.UserInRoleVM.Add(userInRole);
            }
            return rolevm;
        }
        #endregion
    }
}
