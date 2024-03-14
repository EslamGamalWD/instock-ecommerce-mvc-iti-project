using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Interfaces.Domain;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<Cart> GetCart(string userName);
}