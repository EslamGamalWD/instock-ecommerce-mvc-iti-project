using InStockWebAppBLL.Models.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface ICheckOutRepository
    {
        Task<bool> CheckoutEdit(UserCheckoutDetailsVM editedUser);

    }
}
