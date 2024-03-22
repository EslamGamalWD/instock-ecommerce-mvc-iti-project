using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.OrderVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public OrderRepository(ApplicationDbContext applicationDbContext,IMapper mapper)
        {
            this.applicationDbContext=applicationDbContext;
            this.mapper=mapper;
        }
        public async Task<int?> Add(AddOrderVM addOrderVM)
        {
            try
            {
                var result = mapper.Map<Order>(addOrderVM);
                await applicationDbContext.Orders.AddAsync(result);
                await applicationDbContext.SaveChangesAsync();
                return result.Id;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<IEnumerable<GetAllOrderVM>> GetAllOrders(string userId)
        {
            var orders = await applicationDbContext.Orders
                .Include(p => p.PaymentDetails)
                .Include(i=>i.Items)
                .ThenInclude(p=>p.Product)
                .ThenInclude(p => p.Images)
                .Where(a => a.UserId==userId)
                .ToListAsync();
            var result = mapper.Map<IEnumerable<GetAllOrderVM> >(orders);
            return result;
        }
    }
}
