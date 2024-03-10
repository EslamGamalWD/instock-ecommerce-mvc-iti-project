using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class UserPaymentRepository : IUserPaymentRepository
    {
        
        

        #region Prop
        private readonly ApplicationDbContext Db;
        #endregion


        #region ctor
        public UserPaymentRepository(ApplicationDbContext Db)
        {
            this.Db=Db;
        }
        #endregion

        #region Method
        public async Task<bool> AddListPayment(IEnumerable<UserPayment>? payment, string userid)
        {
            try
            {
                foreach (var item in payment)
                {
                    item.UserId = userid;
                    await Db.UserPayments.AddAsync(item);
                    await Db.SaveChangesAsync();
                }
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
