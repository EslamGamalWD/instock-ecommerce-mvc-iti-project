using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Interfaces.Domain;

public interface ICategoryRepository : IGenericRepository<Category>
{
    new Task<bool> Add(Category entity);
    Task<DateTime?> ToggleStatus(int id);
    Task<CategoryDetailsVM> GetCategoryDetailsWithSubCategories(int categoryId);
}
