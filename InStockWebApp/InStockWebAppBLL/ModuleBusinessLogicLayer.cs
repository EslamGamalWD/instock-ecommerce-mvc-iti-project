using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Helpers.Account;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace InStockWebAppBLL
{
    public static class ModuleBusinessLogicLayer
    {
        public static IServiceCollection AddBusinessLogicLayerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUserPaymentRepository, UserPaymentRepository>();
            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
            services.AddScoped<IFilterRepository, FilterRepository>();

            return services;
        }
    }
}
