using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

public class UpdateDiscountVM
{
   
    public int Id { get; set; } 

    [Required(ErrorMessage = "Discount name is required")]
    [MaxLength(100, ErrorMessage = "Max letters is 100!")]
    [DisplayName("Discount Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Discount description is required")]
    [MaxLength(1000, ErrorMessage = "Max letters is 1000!")]
    [DisplayName("Discount Description")]
    public string Description { get; set; }

    [DisplayName("Image Path")]
    public string ImagePath { get; set; }

    [Required(ErrorMessage = "Discount percentage is required")]
    [Range(1, 100, ErrorMessage = "Percentage must be between 1 and 100!")]
    [DisplayName("Discount Percentage")]
    public int Percentage { get; set; }

    [DisplayName("Active Status")]
    public bool IsActive { get; set; }
}
