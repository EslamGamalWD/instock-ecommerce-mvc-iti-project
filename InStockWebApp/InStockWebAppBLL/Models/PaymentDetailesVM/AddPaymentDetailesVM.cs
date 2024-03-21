using InStockWebAppDAL.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.PaymentDetailesVM
{
    public class AddPaymentDetailesVM
    {

        public decimal Amount { get; set; }
        public string Provider { get; set; } = string.Empty;
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
