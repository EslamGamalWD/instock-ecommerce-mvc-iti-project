
using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
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

        public async Task<bool> Add(AlterProductVM entityVM)
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
                    return true;
                }
                return false;
                
            }catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int? id)
        {
            try
            {
                var product = _applicationDbContext.Products.Find(id);
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

        public async Task<GetProductsVM> Details(int? id)
        {
            try
            {
                var DBProduct=await _applicationDbContext.Products
                    .Include(P => P.SubCategory).Include(P => P.Discount).FirstOrDefaultAsync(P=>P.Id==id);
                return _mapper.Map<GetProductsVM>(DBProduct);

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
                    .Include(P=>P.SubCategory).Include(P=>P.Discount).ToListAsync();
                var ShowProducts=_mapper.Map<IEnumerable<GetProductsVM>>(DbProducts);
                return ShowProducts;
            }catch(Exception) 
            { 
                return Enumerable.Empty<GetProductsVM>();
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
    }
}
