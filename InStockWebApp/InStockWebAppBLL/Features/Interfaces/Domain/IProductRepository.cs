﻿using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppDAL.Entities;


namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IProductRepository
    {
        Task<IEnumerable<GetProductsVM>> GetAll();
        Task<int> Add(AlterProductVM entityVM);

        Task<bool> Update(int? id, AlterProductVM entityVM);
        Task<bool> Delete(int? id);
        Task<DateTime?> ToggleStatus(int? id);
        Task<GetProductsVM> Details(int? id);
        Task<AlterProductVM> EditDetails(int? id);
        Task<IEnumerable<Product>> GetProductsBySubcategoryId(int subcategoryId);

    }
}
