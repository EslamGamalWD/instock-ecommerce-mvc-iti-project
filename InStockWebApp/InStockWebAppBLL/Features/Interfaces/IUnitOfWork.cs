using InStockWebAppBLL.Features.Interfaces.Domain;

namespace InStockWebAppBLL.Features.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    ISubCategoryRepository SubcategoryRepository { get; }
    ICartRepository CartRepository { get; }
    IItemRepository ItemRepository { get; }

    Task Save();
}