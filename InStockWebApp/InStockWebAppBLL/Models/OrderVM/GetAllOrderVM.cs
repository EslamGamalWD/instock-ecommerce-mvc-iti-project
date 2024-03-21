using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.OrderVM
{
    public class GetAllOrderVM
    {
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        [ForeignKey("PaymentDetails")]
        public int PaymentDetailsId { get; set; }
        public PaymentDetails PaymentDetails { get; set; } = default!;
        public IEnumerable<Item> Items { get; set; } = default!;

    }
}
