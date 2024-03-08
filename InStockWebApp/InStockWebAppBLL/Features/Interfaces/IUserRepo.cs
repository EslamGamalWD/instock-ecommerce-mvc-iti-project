﻿using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces
{
    public interface IUserRepo
    {
        Task<bool> Create(CreateUserVM createUserVM);
        Task<IEnumerable<GetAllUserVM>> getAll();
    }
}
