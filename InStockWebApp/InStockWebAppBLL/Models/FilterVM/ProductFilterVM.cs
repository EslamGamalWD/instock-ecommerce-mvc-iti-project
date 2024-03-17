using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.FilterVM
{
    public class ProductFilterVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        public int DiscountId { get; set; }
        public Discount? Discount { get; set; }
    }
}
