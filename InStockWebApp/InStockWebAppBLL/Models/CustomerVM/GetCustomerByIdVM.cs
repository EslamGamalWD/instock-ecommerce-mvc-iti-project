using InStockWebAppDAL.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.CustomerVM
{
    public class GetCustomerByIdVM
    {
        string Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; }
        public Gender Gender { get; set; }

    }
}
