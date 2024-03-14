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

    public override void Update(Item entity)
    {
        entity.ModifiedAt = DateTime.Now;
        _applicationDbContext.Entry(entity).State = EntityState.Modified;
    }
}