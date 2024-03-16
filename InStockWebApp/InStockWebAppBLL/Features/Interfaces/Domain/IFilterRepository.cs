using InStockWebAppBLL.Models.FilterVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IFilterRepository
    {
         Task<IEnumerable<ProductFilterVM>> GetProductsForPage(int page, int pageSize);
        Task<int> totalCount();

        Task<IEnumerable<ProductFilterVM>> GetByFilter(int page, int pageSize, string sortOption , int? categoryId , string subcategoryIds , int minPrice , int maxPrice,string search);
    }
}
