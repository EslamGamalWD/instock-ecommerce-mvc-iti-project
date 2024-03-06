namespace InStockWebAppDAL.Entities;

public class Order
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int PaymentDetailsId { get; set; }
    public PaymentDetails PaymentDetails { get; set; } = default!;
    public IEnumerable<Item> Items { get; set; } = default!;
    public IEnumerable<OrderLog> OrderLogs { get; set; } = default!;
}