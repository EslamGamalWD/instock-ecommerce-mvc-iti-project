using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InStockWebAppBLL.Models.Custom_Validation_Attributes;

namespace InStockWebAppBLL.Models.CategoryVM
{
    public class EditCategoryVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "NAME IS REQUIRED!!!")]
        [MaxLength(30, ErrorMessage = "NAME IS TOO LONG!!!!")]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "DESCRIPTION IS REQUIRED!!!")]
        [MaxLength(2000, ErrorMessage = "DESCRIPTION IS TOO LONG!!!!")]
        public string Description { get; set; }

        public string? Message { get; set; } = null;
    }
}
