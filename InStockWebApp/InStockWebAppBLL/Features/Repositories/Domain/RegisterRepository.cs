using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities.Enumerators;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InStockWebAppDAL.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class RegisterRepository: IRegisterRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterRepository(ApplicationDbContext db, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db=db;
            this.mapper=mapper;
            this.userManager=userManager;
            this.roleManager=roleManager;
        }

        public async Task<User> Register(RegisterVM model)
        {
            var user = mapper.Map<User>(model);

            user.UserType = UserType.Customer;
            user.CreatedAt = DateTime.Now;
            user.Photo = "Men.jpg";

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var resultrole = await userManager.AddToRoleAsync(user, AppRoles.Customer);

                return user;
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to register user: {errorMessage}");
            }
        }


        //public async Task<User> Register(RegisterVM model)
        //{
        //    try
        //    {
        //        var user = mapper.Map<User>(model);

        //        user.UserType = UserType.Customer;
        //        user.CreatedAt = DateTime.Now;
        //        user.Photo=(user.Gender ==0 ? "user-1.jpg" : "11.png");

        //        var result = await userManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            var resultrole = await userManager.AddToRoleAsync(user, AppRoles.Customer);

        //            return user;
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                Console.WriteLine(error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error registering user: {ex.Message}");
        //    }

        //    return null;
        //}
    }
}
