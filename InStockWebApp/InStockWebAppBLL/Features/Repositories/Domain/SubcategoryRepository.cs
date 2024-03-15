using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InStockWebAppBLL.Features.Repositories.Domain;

public class SubcategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SubcategoryRepository(ApplicationDbContext applicationDbContext) : base(
        applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    /// <summary>
    /// Adds a new SubCategory to the SubCategory table
    /// in the database and initializes the CreatedAt
    /// property to the current date and time of adding
    /// </summary>
    /// <param name="entity"></param>
    public override async Task Add(SubCategory entity) =>
        await _applicationDbContext.AddAsync(entity);

    public override void Delete(SubCategory entity)
    {
        throw new NotImplementedException();
    }


    public override void Update(SubCategory entity)
    {
        entity.ModifiedAt = DateTime.Now;
        _applicationDbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task<IEnumerable<SubCategory>> getAllSubCategoriesByCategoryId(int id)
    {
        var subcategories = await _applicationDbContext.SubCategories
            .Where(sc => sc.CategoryId == id)
            .ToListAsync();
        return subcategories;
    }
}