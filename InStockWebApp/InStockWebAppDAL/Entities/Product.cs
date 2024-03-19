namespace InStockWebAppDAL.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public decimal AvgRating {  get; set; }
    public int UnitsSold { get; set; }
    public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = default!;
    public int? DiscountId { get; set; }
    public Discount Discount { get; set; } = default!;
    public string? ImgeUrl { get; set; } = "https://placebear.com/g/200/200";

    public virtual List<ProductReview>? Reviews { get; set; }
}