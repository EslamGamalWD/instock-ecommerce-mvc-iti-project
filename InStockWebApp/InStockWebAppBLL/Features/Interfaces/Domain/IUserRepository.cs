﻿using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IUserRepository
    {
        Task<string> Create(CreateUserVM createUserVM);
        Task<IEnumerable<GetAllUserVM>> getAll();
        Task<DateTime?> ToggleStatus(string id);
        Task<GetUserByIdVM> GetUserById(string id);
        Task<bool> Edit(EditUserVM editUserVM);
        Task<User> Register(RegisterVM model);
    }
}
