namespace InStockWebAppDAL.Entities;

public class Item
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSelected { get; set; }
    public int? OrderId { get; set; }
    public Order? Order { get; set; }
    public int? CartId { get; set; }
    public Cart? Cart { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}