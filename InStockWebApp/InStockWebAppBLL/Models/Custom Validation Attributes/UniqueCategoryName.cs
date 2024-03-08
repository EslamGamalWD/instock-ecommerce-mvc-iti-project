using InStockWebAppDAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.Custom_Validation_Attributes
{
    public class UniqueCategoryName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var name = value.ToString();
                var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

                var existingCategory = dbContext.Categories.FirstOrDefault(c => c.Name == name);

                if (existingCategory != null)
                {
                    return new ValidationResult("Category Name Exists!");
                }
            }

            return ValidationResult.Success;
        }
    }
}
