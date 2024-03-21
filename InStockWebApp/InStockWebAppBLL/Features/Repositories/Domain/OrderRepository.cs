using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.OrderVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
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
        public async Task<bool> Add(AddOrderVM addOrderVM)
        {
            try
            {
                var result = mapper.Map<Order>(addOrderVM);
                await applicationDbContext.Orders.AddAsync(result);
                await applicationDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
