using AutoMapper;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using InStockWebAppDAL.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Hangfire;
using InStockWebAppBLL.Features.Interfaces;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class UserRepository : IUserRepository
    {
        #region Prop

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext db;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironmen;
        private readonly IMapper mapper;

        #endregion

        #region Ctor




        public UserRepository(ApplicationDbContext db, IEmailSender emailSender, IWebHostEnvironment webHostEnvironmen, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.emailSender = emailSender;
            this.webHostEnvironmen = webHostEnvironmen;
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
                user.EmailConfirmed = true;
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

                if (user.IsDeleted)
                {
                    #region send Email

                    var filePath = $"{webHostEnvironmen.WebRootPath}/Account/Tempelet/htmlpage.html";
                    StreamReader str = new StreamReader(filePath);
                    var body = str.ReadToEnd();
                    str.Close();
                    body = body.Replace("[Header]", $"No Welcom ya {user.FirstName} {user.LastName}  In Instock Shopping")
                        .Replace("[Body]", "You have violated privacy and been banned ");


                    //Use HangFire
                    BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(user.Email, "Block", body));

                    #endregion
                }

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
                if(editUserVM.Photo != null)
                {
                    user.Photo =editUserVM.Photo;
                }
                if(editUserVM.CityId !=null)
                {
                    user.CityId = editUserVM.CityId;
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        

        public async Task<User> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> UserDataExist(string userId)
        {
            var user = await GetUserById(userId);
            if (user != null &&
                !string.IsNullOrEmpty(user.CityName) &&
                !string.IsNullOrEmpty(user.StateName) &&
                !string.IsNullOrEmpty(user.LastName) &&
                !string.IsNullOrEmpty(user.FirstName) &&
                !string.IsNullOrEmpty(user.PhoneNumber) &&
                !string.IsNullOrEmpty(user.Email))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ConfirmEmail(string id)
        {
            try
            {
                var user = await db.Users.Where(a => a.Id==id).FirstOrDefaultAsync();
                user.EmailConfirmed = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        #endregion
    }
}