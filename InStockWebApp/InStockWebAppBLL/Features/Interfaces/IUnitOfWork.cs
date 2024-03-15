using InStockWebAppBLL.Features.Interfaces.Domain;

namespace InStockWebAppBLL.Features.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    ISubCategoryRepository SubcategoryRepository { get; }

    IDiscountRepository DiscountRepository { get; }

    ICartRepository CartRepository { get; }
    IProductRepository ProductRepository { get; }
    IItemRepository ItemRepository { get; }
    IUserRepository UserRepository { get; }


    Task Save();
}