﻿using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        #endregion

        #region Ctor
        public UserRepo(ApplicationDbContext Db,IMapper mapper,UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            db=Db;
            this.mapper=mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        #endregion

        #region Method
        public async Task< bool> Create(CreateUserVM createUserVM)
        {
            try
            {
                var user = mapper.Map<User>(createUserVM);
                var result = await userManager.CreateAsync(user, createUserVM.PasswordHash);
                string Role = AppRoles.EnumToString(user.UserType);
                var resultrole = await userManager.AddToRoleAsync(user, Role);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

          
        }

        public async Task<IEnumerable<GetAllUserVM>> getAll()
        {

            var users =await db.Set<User>().Include(user => user.City)
            .ThenInclude(city => city.State)
            .ToListAsync();

           
           
            var UsersVM = mapper.Map<IEnumerable<GetAllUserVM>>(users);


            return UsersVM;
        }
        #endregion

    }
}
