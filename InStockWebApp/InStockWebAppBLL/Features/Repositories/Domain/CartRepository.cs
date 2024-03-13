using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InStockWebAppBLL.Features.Repositories.Domain;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public override Task Add(Cart entity)
    {
        throw new NotImplementedException();
    }

    public override void Delete(Cart entity)
    {
        throw new NotImplementedException();
    }

    public override void Update(Cart entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Cart> GetCart(string userName)
    {
        var user = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u =>
                string.Equals(u.UserName, userName));

        return await GetFirstOrDefault(c => string.Equals(c.UserId, user.Id),
                   c => c.Items)
               ?? new Cart
               {
                   UserId = user.Id,
                   User = user
               };
    }
}