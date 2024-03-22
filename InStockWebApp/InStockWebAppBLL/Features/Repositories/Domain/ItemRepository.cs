using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InStockWebAppBLL.Features.Repositories.Domain;

public class ItemRepository : GenericRepository<Item>, IItemRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ItemRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public override async Task Add(Item entity) =>
        await _applicationDbContext.Items.AddAsync(entity);

  
    public override void Delete(Item entity)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> DecreaseUnitStock(int cardId, int? orderId)
    {
        try
        {
            var items = await _applicationDbContext.Items.Where(a => a.CartId == cardId && a.IsDeleted ==false).ToListAsync();
            foreach (var item in items)
            {
                var Product = await _applicationDbContext.Products.Where(a => a.Id ==item.ProductId).FirstOrDefaultAsync();
                Product.InStock -= item.Quantity;
                item.OrderId = orderId;
                await _applicationDbContext.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception)
        {

            return false;
        }
     
    }

    public async Task<bool> IncreaseUnitSold(int cardId)
    {
        try
        {
            var items = await _applicationDbContext.Items.Where(a => a.CartId == cardId && a.IsDeleted ==false).ToListAsync();
            foreach (var item in items)
            {
                var Product = await _applicationDbContext.Products.Where(a => a.Id ==item.ProductId).FirstOrDefaultAsync();
                Product.UnitsSold += item.Quantity;
                await _applicationDbContext.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }

    public override void Update(Item entity)
    {
        entity.ModifiedAt = DateTime.Now;
        _applicationDbContext.Entry(entity).State = EntityState.Modified;
    }
}