using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.PaymentDetailesVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class PaymentDetailesRepository : IPaymentDetailesRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public PaymentDetailesRepository(ApplicationDbContext Db,IMapper mapper)
        {
            db=Db;
            this.mapper=mapper;
        }
        public async Task<int?> Add(AddPaymentDetailesVM addPayment)
        {
            try
            {
                var paymentDetailes = mapper.Map<PaymentDetails>(addPayment);
                await db.PaymentDetails.AddAsync(paymentDetailes);
                await db.SaveChangesAsync();
                return paymentDetailes.Id;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
