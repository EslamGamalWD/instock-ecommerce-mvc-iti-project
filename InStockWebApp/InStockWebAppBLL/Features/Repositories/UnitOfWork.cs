using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;

namespace InStockWebAppBLL.Features.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;
    public ISubCategoryRepository SubCategoryRepository { get; }

    public UnitOfWork(ApplicationDbContext applicationDbContext,
        ISubCategoryRepository subCategoryRepository)
    {
        _applicationDbContext = applicationDbContext;
        SubCategoryRepository = subCategoryRepository;
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