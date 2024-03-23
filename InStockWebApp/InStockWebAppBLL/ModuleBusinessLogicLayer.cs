using InStockWebAppBLL.Mapper;
using Microsoft.Extensions.DependencyInjection;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Helpers.Account;
using Microsoft.AspNetCore.Identity.UI.Services;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Repositories;

namespace InStockWebAppBLL
{
    public static class ModuleBusinessLogicLayer
    {
        public static IServiceCollection AddBusinessLogicLayerDependencies(
            this IServiceCollection services)
        {

            services.AddScoped<IpaymentService, PaymentService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ISubCategoryRepository, SubcategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUserPaymentRepository, UserPaymentRepository>();
            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            services.AddScoped<IRegisterRepository,RegisterRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<ICheckOutRepository, CheckOutRepository>();
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
            services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
            services.AddScoped<IGetCategoryWithProductRepository, GetCategoryWithProductRepository>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentDetailesRepository, PaymentDetailesRepository>();

            return services;
        }
    }
}