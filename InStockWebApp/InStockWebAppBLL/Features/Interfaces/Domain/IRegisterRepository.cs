using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IRegisterRepository
    {
        Task<User> Register(RegisterVM model);

    }
}
