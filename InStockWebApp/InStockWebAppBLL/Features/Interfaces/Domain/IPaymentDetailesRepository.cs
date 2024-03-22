﻿using InStockWebAppBLL.Models.PaymentDetailesVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IPaymentDetailesRepository
    {
        Task<int?> Add(AddPaymentDetailesVM addPayment);
    }
}