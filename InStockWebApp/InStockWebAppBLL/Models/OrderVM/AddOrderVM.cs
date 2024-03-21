using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.OrderVM
{
    public class AddOrderVM
    {
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public string UserId { get; set; }
        public int PaymentDetailsId { get; set; }

    }
}
