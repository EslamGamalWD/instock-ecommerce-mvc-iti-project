using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class CheckOutRepository: ICheckOutRepository
    {
        private readonly ApplicationDbContext db;

        public CheckOutRepository(ApplicationDbContext db)
        {
            this.db=db;
        }
        public async Task<bool> CheckoutEdit(UserCheckoutDetailsVM editUserVM)
        {
            try
            {
                var user = await db.Users.Where(a => a.Id == editUserVM.Id).FirstOrDefaultAsync();
                user.FirstName = editUserVM.FirstName;
                user.LastName = editUserVM.LastName;
                user.Email = editUserVM.Email;
                user.PhoneNumber = editUserVM.PhoneNumber;
                user.ModifiedAt = DateTime.Now;

                user.CityId = editUserVM.CityId;

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
