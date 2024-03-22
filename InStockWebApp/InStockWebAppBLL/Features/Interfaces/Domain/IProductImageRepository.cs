using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IProductImageRepository
    {
        Task<bool> add(IEnumerable<IFormFile> images, int productId);
        Task<bool> remove(IEnumerable<ProductImage> images, int productId);
    }
}
