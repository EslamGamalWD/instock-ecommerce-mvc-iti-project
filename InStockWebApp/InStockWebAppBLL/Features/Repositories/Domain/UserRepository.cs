using AutoMapper;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.UserVM;
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
using InStockWebAppDAL.Context;
using System.Collections;
using System.Diagnostics;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class UserRepository : IUserRepository
    {
        #region Prop

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        #endregion

        #region Ctor

        public UserRepository(ApplicationDbContext db, IMapper mapper,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #endregion

        #region Method

        public async Task<string> Create(CreateUserVM createUserVM)
        {
            try
            {
                var user = mapper.Map<User>(createUserVM);
                var result = await userManager.CreateAsync(user, createUserVM.PasswordHash);
                if (result.Succeeded)
                {
                    string Role = AppRoles.EnumToString(user.UserType);
                    var resultrole = await userManager.AddToRoleAsync(user, Role);
                    return user.Id;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        public async Task<DateTime?> ToggleStatus(string id)
        {
            var user = await db.Users.Where(use => use.Id == id).FirstOrDefaultAsync();
            if (user is { })
            {
                user.IsDeleted = !user.IsDeleted;
                user.ModifiedAt = DateTime.Now;
                await db.SaveChangesAsync();
                return DateTime.Now;
            }

            return null;
        }

        public async Task<IEnumerable<GetAllUserVM>> getAll()
        {
            var users = await db.Set<User>().Include(user => user.City)
                .ThenInclude(city => city.State)
                .ToListAsync();
            var UsersVM = mapper.Map<IEnumerable<GetAllUserVM>>(users);
            return UsersVM;
        }
        
        public async Task<GetUserByIdVM> GetUserById(string id)
        {
            var users = await db.Users.Where(a => a.Id == id).Include(user => user.City)
                .ThenInclude(city => city.State).FirstOrDefaultAsync();
            var UsersVM = mapper.Map<GetUserByIdVM>(users);
            return UsersVM;
        }

        public async Task<User?> GetUser(string id) =>
            await db.Users.Where(a => a.Id == id).Include(user => user.City)
                .ThenInclude(city => city.State).FirstOrDefaultAsync();

        public async Task<bool> Edit(EditUserVM editUserVM)
        {
            try
            {
                var user = await db.Users.Where(a => a.Id == editUserVM.Id).FirstOrDefaultAsync();
                user.FirstName = editUserVM.FirstName;
                user.LastName = editUserVM.LastName;
                user.Email = editUserVM.Email;
                user.Gender = editUserVM.Gender;
                user.PhoneNumber = editUserVM.PhoneNumber;
                user.ModifiedAt = DateTime.Now;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User> Register(RegisterVM model)
        {
            try
            {
                var user = mapper.Map<User>(model);

                user.UserType = UserType.Customer;
                user.CreatedAt = DateTime.Now;

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    string role = AppRoles.EnumToString(user.UserType);
                    var roleResult = await userManager.AddToRoleAsync(user, role);
                    return user;
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
            }

            return null;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }
        #endregion
    }
}