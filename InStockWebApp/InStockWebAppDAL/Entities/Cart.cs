namespace InStockWebAppDAL.Entities;

public class Cart
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public IEnumerable<Item>? Items { get; set; }
    public string? CheckoutSessionId { get; set; }
}