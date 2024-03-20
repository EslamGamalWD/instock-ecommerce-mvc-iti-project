using System.Reflection;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InStockWebAppDAL.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Discount> Discounts { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderLog> OrderLogs { get; set; }
    public virtual DbSet<PaymentDetails> PaymentDetails { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductReview> ProductReviews { get; set; }
    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<SubCategory> SubCategories { get; set; }
    public virtual DbSet<UserPayment> UserPayments { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ProductImage> Images { get; set; }
    public virtual DbSet<ContactMessage> ContactMessage { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}