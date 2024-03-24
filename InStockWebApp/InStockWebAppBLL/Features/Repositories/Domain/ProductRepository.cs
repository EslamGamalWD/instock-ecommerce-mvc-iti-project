
using AutoMapper;
using Azure;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class ProductRepository :IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext applicationDbContext,IMapper mapper) 
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<int> Add(AlterProductVM entityVM)
        {
            try
            {
                //Do We need to check on the existance of the product??
                //Maybe make it return int so 0==successful ,1== mapping failed ,2== saving Changes failed.
                var product = _mapper.Map<Product>(entityVM);
                if (product != null)
                {
                    product.CreatedAt = DateTime.Now;
                    _applicationDbContext.Products.Add(product);
                    await _applicationDbContext.SaveChangesAsync();
                    return product.Id;
                }
                return -1;
                
            }catch (Exception)
            {
                return -1;
            }
        }


        public async Task<GetProductsVM> Details(int? id)
        {
            try
            {
                var DBProduct = await _applicationDbContext.Products
                 .Include(P => P.SubCategory)
                 .Include(P => P.Discount)
                 .Include(P => P.Images)
                 .Include(p => p.Reviews)
                 .ThenInclude(r => r.User)
                 .FirstOrDefaultAsync(P => P.Id == id);
                GetProductsVM getProductsVM = new GetProductsVM()
                {
                    Id = DBProduct.Id,
                    Name = DBProduct.Name,
                    Description = DBProduct.Description,
                    Price = DBProduct.Price,
                    InStock=DBProduct.InStock,
                    AvgRating = DBProduct.AvgRating,
                    CreatedAt = DBProduct.CreatedAt,
                    IsDeleted = DBProduct.IsDeleted,
                    SubCategoryName = DBProduct.SubCategory.Name,
                    DiscountName = DBProduct.Discount?.Name??"",
                    ProductReviews = DBProduct.Reviews
                };
                foreach(var img in DBProduct.Images)
                {
                    getProductsVM.ImagePaths.Add( img.ImagePath);
                }
                //return _mapper.Map<GetProductsVM>(DBProduct);
                return getProductsVM;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AlterProductVM> EditDetails(int? id)
        {
            try
            {
                var DBProduct = await _applicationDbContext.Products
                    .Include(P => P.SubCategory).Include(P => P.Discount).Include(P => P.Images).FirstOrDefaultAsync(P => P.Id == id);
                AlterProductVM AlterProductsVM = new AlterProductVM()
                {
                    Id = DBProduct.Id,
                    Name = DBProduct.Name,
                    Description = DBProduct.Description,
                    Price = DBProduct.Price,
                    InStock = DBProduct.InStock,
                    SubCategoryId = DBProduct.SubCategory.Id,
                    DiscountId= DBProduct.Discount?.Id
                };
                AlterProductsVM.Images = new();
                for (int i = 0; i < DBProduct.Images.Count; i++)
                {
                    ProductImage? img = DBProduct.Images[i];
                    AlterProductsVM?.Images.Add(img);
                }
                // return _mapper.Map<AlterProductVM>(DBProduct);
                return AlterProductsVM;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<IEnumerable<GetProductsVM>> GetAll()
        {
            try
            {
                var DbProducts= await _applicationDbContext.Products
                    .Include(P=>P.SubCategory).Include(P=>P.Discount).Include(P => P.Images).ToListAsync();
                //var ShowProducts=_mapper.Map<IEnumerable<GetProductsVM>>(DbProducts);
                List<GetProductsVM> ShowProducts= new List<GetProductsVM>() { };
                foreach (var DBProduct in DbProducts)
                {
                    GetProductsVM getProductsVM = new GetProductsVM()
                    {
                        Id = DBProduct.Id,
                        Name = DBProduct.Name,
                        Description = DBProduct.Description,
                        Price = DBProduct.Price,
                        InStock = DBProduct.InStock,
                        AvgRating = DBProduct.AvgRating,
                        CreatedAt = DBProduct.CreatedAt,
                        IsDeleted = DBProduct.IsDeleted,
                        SubCategoryName = DBProduct.SubCategory.Name,
                        DiscountName = DBProduct.Discount?.Name??"",
                        ProductReviews = DBProduct?.Reviews
                    };
                    foreach (var img in DBProduct.Images)
                    {
                        getProductsVM.ImagePaths.Add(img.ImagePath);
                    }
                    ShowProducts.Add(getProductsVM);
                }
                return ShowProducts;
            }catch(Exception) 
            { 
                return Enumerable.Empty<GetProductsVM>();
            }
        }
        public async Task<bool> Delete(int? id)
        {
            try
            {
                var product = await _applicationDbContext.Products.FindAsync(id);
                if (product != null)
                {
                    _applicationDbContext.Products.Remove(product);
                    _applicationDbContext.SaveChanges();
                    return true;
                }
                return false;

            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<DateTime?> ToggleStatus(int? id)
        {
            try
            {
                var product = _applicationDbContext.Products.Find(id);
                if (product != null)
                {
                    product.IsDeleted = !product.IsDeleted;
                    var now=DateTime.Now;
                    if(product.IsDeleted)product.DeletedAt= now;
                    else
                    {
                        product.DeletedAt = null;
                    }
                    await _applicationDbContext.SaveChangesAsync();
                    return now;
                }
                return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> Update(int? id, AlterProductVM entityVM)
        {
            try
            {
                var DBProduct=_mapper.Map<Product>(entityVM);
                if(DBProduct != null)
                {
                    _applicationDbContext.Update(DBProduct);
                    await _applicationDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsBySubcategoryId(int subcategoryId)
        {
            var products = await _applicationDbContext.Products
                .Include(p => p.SubCategory)
                .Include(p => p.Discount)
                .Include(p => p.Images)
                .Where(p => p.SubCategoryId == subcategoryId && !p.IsDeleted)
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsWithActiveDiscount()
        {
            var productsWithDiscount = await _applicationDbContext.Products
                .Include(p => p.SubCategory)
                .Include(p => p.Discount)
                .Include(p => p.Images)
                .Where(p => !p.IsDeleted && p.DiscountId != null &&
                            !p.Discount.IsDeleted && p.Discount.IsActive)
                .ToListAsync();

            return productsWithDiscount;
        }

        public async Task<IEnumerable<Product>> GetProductsOrderedByUnitsSold()
        {
            var productsOrderedByUnitsSold = await _applicationDbContext.Products
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.UnitsSold)
                .ToListAsync();

            return productsOrderedByUnitsSold;
        }

        public async Task<Product?> GetProductWithSubcategoryById(int id)
        {
            try
            {
                var product = await _applicationDbContext.Products
                    .Include(p => p.SubCategory)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<Product?> GetById(int id) =>
            await _applicationDbContext.Products
                .Include(p => p.SubCategory)
                .Include(p => p.Discount)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<int> GetAllProductSold()
        {
            var count =await _applicationDbContext.Products.Where(p => p.InStock==0).CountAsync();
            return count;
        }
    }
}
