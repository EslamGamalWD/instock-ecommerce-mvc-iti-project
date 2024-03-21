using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.FilterVM;
using InStockWebAppDAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class FilterRepository : IFilterRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public FilterRepository(ApplicationDbContext applicationDbContext,IMapper mapper)
        {
            this.applicationDbContext=applicationDbContext;
            this.mapper=mapper;
        }

        public async Task<IEnumerable<ProductFilterVM>> GetByFilter(int page, int pageSize, string sortOption, int? categoryId, string subcategoryIds, int minPrice, int maxPrice,string search)
        {

            var products = applicationDbContext.Products.Include(a=>a.Images).AsQueryable();
            if(search !=null)
            {
                if (decimal.TryParse(search, out decimal price))
                {
                    products = products.Where(p => p.Price == price ||p.Name.Contains(search));
                }
                else
                {
                    products = products.Where(p => p.Name.Contains(search) );
                }
            }
            var test = products.ToList();
            if (categoryId != null)
            {
                products = products.Where(p => p.SubCategory.CategoryId == categoryId);
            }
            List<int> subcatigeries = new List<int>();
            string[] subcategoryIdsArray = subcategoryIds?.Split(',');
            for (int i = 0; i < subcategoryIdsArray?.Length; i++)
            {


                subcatigeries.Add(int.Parse(subcategoryIdsArray[i].ToString()));
            }
            if (subcategoryIds != null && subcategoryIds?.Length > 0)
            {
                products = products.Where(p => subcatigeries.Contains(p.SubCategoryId));
            }
            products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

            switch (sortOption)
            {
                case "popularity":
                    products = products.Include(a => a.Images).OrderBy(p => p.Name);

                    break;
                case "low-high":
                    products = products.Include(a => a.Images).OrderBy(p => p.Price);
                    break;
                case "high-low":
                    products = products.Include(a => a.Images).OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.Include(a => a.Images).OrderBy(p => p.Id);
                    break;
            }

         
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return  mapper.Map<IEnumerable<ProductFilterVM>>(paginatedProducts);
        }

        public async Task<IEnumerable<ProductFilterVM>> GetProductsForPage(int page, int pageSize)
        {
            var products =await applicationDbContext.Products.Include(a=>a.SubCategory).Include(a => a.Images).OrderBy(p => p.Id)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
            var productVM = mapper.Map<IEnumerable<ProductFilterVM>>(products);
            return productVM;
        }

        public async Task<int> totalCount()
        {
            return await applicationDbContext.Products.CountAsync();
        }
    }
}
