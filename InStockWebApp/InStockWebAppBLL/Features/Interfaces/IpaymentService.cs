using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces
{
    public interface IpaymentService
    {
      
        Task<string?> CreatePaymentSession(string cartId ,string Link);
       
    }
}
