using AutoMapper;
using InStockWebAppBLL.Models.CartVM;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppBLL.Models.FilterVM;
using InStockWebAppBLL.Models.RoleVM;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Identity;
using InStockWebAppBLL.Models.ReviewVM;
using InStockWebAppBLL.Models.PaymentDetailesVM;
using InStockWebAppBLL.Models.OrderVM;

namespace InStockWebAppBLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<CreateRoleVM, IdentityRole>();
            CreateMap<IdentityRole, GetAllRoleVM>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<SubCategory, SubcategoryVM>();
            CreateMap<SubcategoryVM, SubCategory>();
            CreateMap<Cart, CartVM>();


            CreateMap<User, GetAllUserVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.City.State.Name));


            CreateMap<CreateUserVM, User>()

                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId));
            CreateMap<UserCheckoutDetailsVM, User>()

                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId));
            CreateMap<GetUserByIdVM, UserCheckoutDetailsVM>();
            CreateMap<CreateCategoryVM, Category>();
            CreateMap<Category, EditCategoryVM>();
            CreateMap<EditCategoryVM, Category>();
            CreateMap<Category, GetAllCategoriesVM>();
            CreateMap<User, GetUserByIdVM>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.City.State.Name));

            CreateMap<Product, GetProductsVM>()
             .ForMember(dest => dest.ImagePaths, opt => opt.MapFrom(src => src.Images.Select(img => img.ImagePath).ToList()))
             .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
             .ForMember(dest => dest.DiscountName, opt => opt.MapFrom(src => src.Discount.Name));
            CreateMap<Product, AlterProductVM>();
            CreateMap<AlterProductVM,Product>();


            CreateMap<GetUserByIdVM, EditUserVM>();
            CreateMap<EditUserVM, User>();


            CreateMap<RegisterVM, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId));


            #region Discount
            CreateMap<CreateDiscountVM, Discount>();
            CreateMap<Discount, GetDiscountByIdVM>()
                      .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
            CreateMap<UpdateDiscountVM, Discount>()
             .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());
            CreateMap<Discount, UpdateDiscountVM>();
            //    .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore()); 
            CreateMap<Discount, GetAllDiscountsVM>();
            CreateMap<Discount, CreateDiscountVM>();
            CreateMap<GetDiscountByIdVM, Discount>();

            CreateMap<Product, GetProductsVM>(); 
            #endregion

            CreateMap<Product, ProductFilterVM>();
            CreateMap<ProductFilterVM, Product>();


            CreateMap<ReviewVM, ProductReview>();


            CreateMap<PaymentDetails, AddPaymentDetailesVM>().ReverseMap();
            CreateMap<Order, AddOrderVM>().ReverseMap();
            CreateMap<Order, GetAllOrderVM>().ReverseMap();

        }
    }
}