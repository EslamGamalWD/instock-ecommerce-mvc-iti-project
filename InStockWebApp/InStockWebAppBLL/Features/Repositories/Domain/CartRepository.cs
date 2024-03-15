using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InStockWebAppBLL.Features.Repositories.Domain;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IHttpContextAccessor _iHttpContextAccessor;
    private readonly UserManager<User> _userManager;

    public CartRepository(ApplicationDbContext applicationDbContext,
        IHttpContextAccessor iHttpContextAccessor, UserManager<User> userManager) : base(
        applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _iHttpContextAccessor = iHttpContextAccessor;
        _userManager = userManager;
    }

    public override async Task Add(Cart entity) =>
        await _applicationDbContext.Carts.AddAsync(entity);

    public override void Delete(Cart entity)
    {
        throw new NotImplementedException();
    }

    public override void Update(Cart entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Cart> GetCart(string userId)
    {
        var cart = await _applicationDbContext.Carts
            .Where(c => string.Equals(c.UserId, userId) && !c.IsDeleted)
            .Include(c => c.Items.Where(i => !i.IsDeleted))
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync();
        return cart;
    }


    public async Task<int> GetCartItemsCount(string userId)
    {
        var count = await _applicationDbContext.Carts
            .Where(c => c.UserId == userId)
            .Include(c => c.Items)
            .SelectMany(c => c.Items.Where(i => !i.IsDeleted))
            .Select(i => i.Quantity)
            .SumAsync();
        return count;
    }

    private decimal CalculateCartTotalPrice(Cart cart) =>
        cart.Items.Sum(i => i.TotalPrice);
}