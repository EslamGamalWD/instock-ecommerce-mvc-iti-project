using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InStockWebAppDAL.Entities
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public string ImagePath { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
