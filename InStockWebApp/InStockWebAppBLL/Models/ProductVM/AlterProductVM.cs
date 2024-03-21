using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace InStockWebAppBLL.Models.ProductVM
{
    public class AlterProductVM
    {
        public int Id { get; set; }
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

        //// FileSize attribute limits the size of the file to 5 MB
        //[MaxFileSize(5 * 1024 * 1024, ErrorMessage = "File size cannot exceed 5 MB.")]
        //// FileExtensions attribute restricts the allowed file types to JPEG, PNG
        //[FileExtensions(Extensions = ".jpg,.jpeg,.png", ErrorMessage = "Only JPEG, PNG and JPG files are allowed.")]
        //public IEnumerable<IFormFile?>? ImageFiles { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();

        [Display(Name = "Subcategory")]
        [Required(ErrorMessage = "Please select a subcategory.")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Discount")]
        public int? DiscountId { get; set; }
    }
}
