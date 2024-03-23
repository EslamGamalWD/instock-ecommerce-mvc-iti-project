using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.HomeVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{

    public class GetCategoryWithProductRepository:IGetCategoryWithProductRepository
    {
        private readonly ISubCategoryRepository subCategoryRepository;
        private readonly IProductRepository productRepository;

        public GetCategoryWithProductRepository(ISubCategoryRepository subCategoryRepository,IProductRepository productRepository)
        {
            this.subCategoryRepository=subCategoryRepository;
            this.productRepository=productRepository;
        }

        public async Task<IEnumerable<CategoryWithProductsVM>> CategoryWithProducts(IEnumerable<Category> categories)
        {
            var categoriesWithProductsVMs = new List<CategoryWithProductsVM>();

            foreach (var category in categories)
            {
                if (category.IsDeleted == true)
                {
                    continue;
                }

                var categoryWithProductsVM = new CategoryWithProductsVM
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImagePath = category.ImagePath,
                    CreatedAt = category.CreatedAt,
                    ModifiedAt = category.ModifiedAt,
                    DeletedAt = category.DeletedAt,
                    IsDeleted = category.IsDeleted,
                    SubCategories =
                        await subCategoryRepository.getAllSubCategoriesByCategoryId(category.Id)
                };

                if (categoryWithProductsVM.SubCategories != null)
                {
                    foreach (var subcategory in categoryWithProductsVM.SubCategories)
                    {
                        if (subcategory.IsDeleted == true)
                        {
                            continue;
                        }

                        subcategory.Products =
                            await productRepository.GetProductsBySubcategoryId(subcategory.Id);

                        if (subcategory.Products != null)
                        {
                            categoryWithProductsVM.Products ??= new List<Product>();
                            categoryWithProductsVM.Products.AddRange(subcategory.Products);
                        }
                    }
                }

                if (categoryWithProductsVM.Products != null)
                {
                    var rnd = new Random();
                    categoryWithProductsVM.Products =
                        categoryWithProductsVM.Products.OrderBy(p => rnd.Next()).ToList();
                }

                categoriesWithProductsVMs.Add(categoryWithProductsVM);
            }
            return categoriesWithProductsVMs;
        }
    }
}
