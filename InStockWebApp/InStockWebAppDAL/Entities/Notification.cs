namespace InStockWebAppDAL.Entities;

public class Notification
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string Content { get; set; } = string.Empty;
    public int OrderLogId { get; set; }
    public OrderLog OrderLog { get; set; } = default!;
}