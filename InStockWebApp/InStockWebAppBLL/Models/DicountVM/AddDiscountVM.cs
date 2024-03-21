using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class CreateDiscountVM
{
    [Required(ErrorMessage = "Discount name is required")]
    [MaxLength(100, ErrorMessage = "Max letters is 100!")]
    [DisplayName("Discount Name")]
    public string Name { get; set; } = "Discount";

    [Required(ErrorMessage = "Discount description is required")]
    [MaxLength(1000, ErrorMessage = "Max letters is 1000!")]
    public string Description { get; set; } = string.Empty ;

    [DisplayName("Image Path")]
    public string? ImagePath { get; set; }
    public IFormFile? Image { get; set; }
    [Required(ErrorMessage = "Discount percentage is required")]
    [Range(1, 100, ErrorMessage = "Percentage must be between 1 and 100!")]
    public int Percentage { get; set; } = 0;

    [DisplayName("Active")]
    public bool IsActive { get; set; } = false;

    
}
