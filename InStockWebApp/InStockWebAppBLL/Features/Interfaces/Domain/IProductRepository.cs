using InStockWebAppBLL.Models.ProductVM;


namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IProductRepository
    {
        Task<IEnumerable<GetProductsVM>> GetAll();
        Task<bool> Add(AlterProductVM entityVM);

        Task<bool> Update(int? id, AlterProductVM entityVM);
        Task<bool> Delete(int? id);
        Task<GetProductsVM> Details(int? id);

    }
}
