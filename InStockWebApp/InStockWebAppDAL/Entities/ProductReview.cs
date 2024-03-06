namespace InStockWebAppDAL.Entities;

public class ProductReview
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Review { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}