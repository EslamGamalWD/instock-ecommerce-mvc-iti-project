using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Interfaces.Domain;

public interface ISubCategoryRepository : IGenericRepository<SubCategory>
{
    Task<IEnumerable<SubCategory>> getAllSubCategoriesByCategoryId(int id);
}