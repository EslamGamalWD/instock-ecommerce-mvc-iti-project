using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL
{
    public static class ModuleBusinessLogicLayer
    {
        public static IServiceCollection AddBusinessLogicLayerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));


            return services;
        }
    }
}
