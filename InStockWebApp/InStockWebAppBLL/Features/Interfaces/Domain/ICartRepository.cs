using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Interfaces.Domain;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task AddItem(int productId, int quantity, User user);
    Task<int> RemoveItem(int productId);
    Task<Cart> GetUserCart();
    Task<int> GetCartItemsCount(string userId = "");
    Task<Cart> GetCart(string userId);
}