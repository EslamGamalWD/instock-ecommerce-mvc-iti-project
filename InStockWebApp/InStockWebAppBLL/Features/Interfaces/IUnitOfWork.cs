using InStockWebAppBLL.Features.Interfaces.Domain;

namespace InStockWebAppBLL.Features.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISubCategoryRepository SubCategoryRepository { get; }

    Task Save();
}