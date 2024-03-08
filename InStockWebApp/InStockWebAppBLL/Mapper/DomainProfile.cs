using AutoMapper;
using InStockWebAppBLL.Models.RoleVM;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace InStockWebAppBLL.Mapper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<CreateRoleVM, IdentityRole>();
            CreateMap<IdentityRole, GetAllRoleVM>();
            CreateMap<SubCategory, SubCategoryVM>();
        }
    }
}
