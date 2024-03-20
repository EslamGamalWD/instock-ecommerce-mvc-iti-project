using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.CategoryVM
{
    public record GetAllCategoriesVM(int Id, string Name, string Description, string? ImagePath, DateTime CreatedAt, DateTime? ModifiedAt, DateTime? DeletedAt, bool IsDeleted);
}
