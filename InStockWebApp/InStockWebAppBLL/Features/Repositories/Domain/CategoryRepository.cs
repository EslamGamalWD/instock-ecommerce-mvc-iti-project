using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InStockWebAppBLL.Features.Repositories.Domain;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CategoryRepository(ApplicationDbContext applicationDbContext) : base(
        applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    //public override async Task Add(Category entity)
    //{
    //    var newCategory = new Category
    //    {
    //        Name = entity.Name,
    //        Description = entity.Description,
    //        CreatedAt = DateTime.Now,
    //        IsDeleted = false
    //    };

    //    await _applicationDbContext.Set<Category>().AddAsync(newCategory);
    //}

    public override async Task<bool> Add(Category entity)
    {
        bool categoryExists = await _applicationDbContext.Categories.AnyAsync(c => c.Name == entity.Name);

        if (!categoryExists)
        {
            var newCategory = new Category
            {
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _applicationDbContext.Set<Category>().AddAsync(newCategory);

            return true;
        }

        return false;
    }

    public override void Delete(Category entity)
    {
        _applicationDbContext.Set<Category>().Remove(entity);
    }

    public override void Update(Category entity)
    {
        var updatedCategory = _applicationDbContext.Categories.Find(entity.Id);

        if (updatedCategory != null)
        {
            updatedCategory.Name = entity.Name;
            updatedCategory.Description = entity.Description;
            updatedCategory.ModifiedAt = DateTime.Now;
        }
    }

    public async Task<DateTime?> ToggleStatus(int id)
    {
        var category = await _applicationDbContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        
        if (category is { })
        {
            category.IsDeleted = !category.IsDeleted;
            category.ModifiedAt = DateTime.Now;
            await _applicationDbContext.SaveChangesAsync();
            return DateTime.Now;
        }

        return null;
    }
}
