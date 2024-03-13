using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;

namespace InStockWebAppBLL.Features.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;
    public ICategoryRepository CategoryRepository { get; }
    public ISubCategoryRepository SubcategoryRepository { get; }
    public IProductRepository ProductRepository { get; }

    public UnitOfWork(ApplicationDbContext applicationDbContext,
        ICategoryRepository categoryRepository, ISubCategoryRepository subcategoryRepository, IProductRepository productRepository)
    {
        _applicationDbContext = applicationDbContext;
        CategoryRepository = categoryRepository;
        SubcategoryRepository = subcategoryRepository;
        ProductRepository = productRepository;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task Save() =>
        await _applicationDbContext.SaveChangesAsync();

    protected virtual async void Dispose(bool disposing)
    {
        if (disposing)
            await _applicationDbContext.DisposeAsync();
    }
}