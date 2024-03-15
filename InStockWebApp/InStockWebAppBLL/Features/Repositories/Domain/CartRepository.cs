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

    public async Task AddItem(int productId, int quantity, User user)
    {
        try
        {
            await using var transaction =
                await _applicationDbContext.Database.BeginTransactionAsync();
            // if (string.IsNullOrEmpty(userId))
            //     throw new Exception("User is not logged-in");

            var cart = await GetCart(user.Id);
            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = user.Id,
                    User = user
                };
                await Add(cart);
            }

            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(p =>
                p.Id == productId);
            var item = await _applicationDbContext.Items.FirstOrDefaultAsync(i =>
                i.ProductId == productId && i.CartId == cart.Id && !i.IsSelected && !i.IsDeleted);
            if (item is null)
            {
                item = new Item
                {
                    ProductId = productId,
                    Product = product,
                    Cart = cart,
                    CartId = cart.Id,
                    Quantity = quantity,
                    TotalPrice = quantity * product.Price
                };
                await _applicationDbContext.Items.AddAsync(item);
            }
            else
            {
                item.Quantity += quantity;
                item.TotalPrice += quantity * product.Price;
            }

            cart.TotalPrice = await CalculateCartTotalPrice(cart.Id);
            await _applicationDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            // ignored
        }

        // return await GetCartItemsCount(user.Id);
    }

    public async Task<int> RemoveItem(int productId)
    {
        var userId = GetUserId();
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");

            var cart = await GetCart(userId);
            if (cart is null)
                throw new Exception("Invalid cart");

            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(p =>
                p.Id == productId);
            var item = await _applicationDbContext.Items.FirstOrDefaultAsync(i =>
                i.ProductId == productId && i.CartId == cart.Id && !i.IsSelected && !i.IsDeleted);
            if (item is null)
                throw new Exception("Product is not found in the cart");
            else if (item.Quantity == 1)
            {
                item.DeletedAt = DateTime.Now;
                item.IsDeleted = true;
            }

            item.Quantity -= 1;
            item.TotalPrice -= item.Quantity * product.Price;
            await _applicationDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // ignored
        }

        return await GetCartItemsCount(userId);
    }

    public async Task<Cart> GetUserCart()
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            throw new Exception("User is not logged-in");

        return await GetCart(userId);
    }

    public async Task<Cart> GetCart(string userId)
    {
        var cart = await GetFirstOrDefault(c => string.Equals(c.UserId, userId) && !c.IsDeleted,
            c => c.Items);
        return cart;
    }

    private string? GetUserId()
    {
        var principal = _iHttpContextAccessor.HttpContext?.User;
        var userId = _userManager.GetUserId(principal);
        return "97375f38-636c-49f9-a8e4-aab67ffd2c16";
    }

    private async Task<decimal> CalculateCartTotalPrice(int cartId)
    {
        var cart = await _applicationDbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        return cart.Items.Sum(i => i.TotalPrice);
    }


    public async Task<int> GetCartItemsCount(string userId = "")
    {
        if (!string.IsNullOrEmpty(userId))
            userId = GetUserId();
        return await _applicationDbContext.Carts
            .Include(c => c.Items)
            .SelectMany(c => c.Items)
            .Select(i => i.Quantity)
            .SumAsync();
    }
}