using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.ReviewVM
{
    public class ReviewVM
    {
        public int Rating { get; set; }
        public string Review { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public int ProductId { get; set; }
    }
}
