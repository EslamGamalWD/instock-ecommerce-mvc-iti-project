using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Models.CartVM;

public class CartVM
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public IEnumerable<Item> Items { get; set; }= new List<Item>();
}