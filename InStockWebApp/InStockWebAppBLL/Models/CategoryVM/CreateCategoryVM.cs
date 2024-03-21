using InStockWebAppBLL.Models.Custom_Validation_Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.CategoryVM
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage = "NAME IS REQUIRED!!!")]
        [UniqueCategoryName]
        [MaxLength(30, ErrorMessage = "NAME IS TOO LONG!!!!")]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "DESCRIPTION IS REQUIRED!!!")]
        [MaxLength(2000, ErrorMessage = "DESCRIPTION IS TOO LONG!!!!")]
        public string Description { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string ImagePath {  get; set; }

        public string? Message { get; set; } = null;
    }
}
