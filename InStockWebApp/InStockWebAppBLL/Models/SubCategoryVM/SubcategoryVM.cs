using System.ComponentModel.DataAnnotations;
using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Models.SubCategoryVM;

public class SubcategoryVM
{
    public int Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "The subcategory name must be 3 characters at least.")]
    [MaxLength(50, ErrorMessage = "The subcategory name must be 50 characters at most.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500, ErrorMessage = "The subcategory name must be 500 characters at most.")]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }

    [Required] public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public IEnumerable<Product>? Products { get; set; }
}