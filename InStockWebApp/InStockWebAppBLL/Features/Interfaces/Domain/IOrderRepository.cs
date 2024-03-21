using InStockWebAppBLL.Models.OrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IOrderRepository
    {

        Task<int?> Add(AddOrderVM addOrderVM);
        Task<IEnumerable<GetAllOrderVM>> GetAllOrders(string userId);
    }
}
