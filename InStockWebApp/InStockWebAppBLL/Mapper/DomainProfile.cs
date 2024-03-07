using AutoMapper;
using InStockWebAppBLL.Models.RoleVM;
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
        }
    }
}
