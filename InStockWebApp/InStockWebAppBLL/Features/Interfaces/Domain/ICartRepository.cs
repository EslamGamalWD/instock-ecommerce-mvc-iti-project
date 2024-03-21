using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Interfaces.Domain;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<int> GetCartItemsCount(string userId);
    Task<Cart> GetCart(string userId);
    decimal CalculateCartTotalPrice(Cart cart);
    Task<bool> EmptyCart(int id);
}