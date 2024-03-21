using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Interfaces.Domain;

public interface IItemRepository : IGenericRepository<Item>
{
   Task<bool> DecreaseUnitStock(int cardId);
    Task<bool> IncreaseUnitSold(int cardId);

}