﻿using InStockWebAppBLL.Models.RoleVM;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IRoleRepository
    {
        Task<bool> Create(CreateRoleVM roleVM);
        Task<IEnumerable<GetAllRoleVM>> GetAll();
    }
}
