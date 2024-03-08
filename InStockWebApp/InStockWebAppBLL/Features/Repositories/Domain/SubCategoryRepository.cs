using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Repositories.Domain;

public class SubCategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SubCategoryRepository(ApplicationDbContext applicationDbContext) : base(
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
        throw new NotImplementedException();
    }
}