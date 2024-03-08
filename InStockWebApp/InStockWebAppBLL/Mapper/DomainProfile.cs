using AutoMapper;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppBLL.Models.RoleVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Mapper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<CreateRoleVM, IdentityRole>();
            CreateMap<IdentityRole, GetAllRoleVM>();

            CreateMap<CreateCategoryVM, Category>();
            CreateMap<Category, EditCategoryVM>();
            CreateMap<EditCategoryVM, Category>();
            CreateMap<Category, GetAllCategoriesVM>();
        }
    }
}
