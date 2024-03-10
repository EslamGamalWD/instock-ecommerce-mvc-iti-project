using System.ComponentModel.DataAnnotations;


namespace InStockWebAppBLL.Models.ProductVM
{
    public class AlterProductVM
    {
        [Required(ErrorMessage = "Please enter the product name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the product description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the product price.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter the initial stock quantity.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be a positive number.")]
        public int InStock { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Display(Name = "Subcategory")]
        [Required(ErrorMessage = "Please select a subcategory.")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Discount")]
        [Required(ErrorMessage = "Please select a discount.")]
        public int DiscountId { get; set; }
    }
}
