using InStockWebAppBLL.Features.Interfaces.Domain;

namespace InStockWebAppBLL.Features.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    ISubCategoryRepository SubCategoryRepository { get; }

    Task Save();
}