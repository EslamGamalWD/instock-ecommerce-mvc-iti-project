using AutoMapper;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppBLL.Models.RoleVM;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace InStockWebAppBLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<CreateRoleVM, IdentityRole>();
            CreateMap<IdentityRole, GetAllRoleVM>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<SubCategory, SubCategoryVM>();
            CreateMap<SubCategoryVM, SubCategory>();


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

            CreateMap<CreateCategoryVM, Category>();
            CreateMap<Category, EditCategoryVM>();
            CreateMap<EditCategoryVM, Category>();
            CreateMap<Category, GetAllCategoriesVM>();
            CreateMap<User, GetUserByIdVM>()
                
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.City.State.Name));
        }
    }
}