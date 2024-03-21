using InStockWebAppDAL.Entities;
using Stripe.Climate;

namespace InStockWebAppBLL.Features.Interfaces.Domain;

public interface IItemRepository : IGenericRepository<Item>
{
   Task<bool> DecreaseUnitStock(int cardId,int? orderId);
    Task<bool> IncreaseUnitSold(int cardId);

}