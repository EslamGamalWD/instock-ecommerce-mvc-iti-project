using InStockWebAppDAL.Entities.Enumerators;

namespace InStockWebAppDAL.Entities;

public class OrderLog
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public IEnumerable<Notification> Notifications { get; set; } = default!;
}