using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
       Task<List<GetAllDiscountsVM>> GetAllDiscounts();
        Task<DateTime?> ToggleStatus(int id);
         Task<GetDiscountByIdVM> GetDiscountById(int id);

        Task<List<GetProductsVM>> GetAllProducts();
        Task<bool> ToggleDiscountAssociation(int productId, int? discountId);

    }
}
