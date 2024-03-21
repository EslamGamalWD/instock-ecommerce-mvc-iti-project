using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class DiscountRepository : GenericRepository<Discount> , IDiscountRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DiscountRepository(ApplicationDbContext applicationDbContext , IMapper mapper) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public override async Task<bool> Add(Discount addedDiscount)
        {
            bool discountExists = await _applicationDbContext.Discounts.AnyAsync(c => c.Name == addedDiscount.Name);

            if (!discountExists)
            {
                var newDiscount = new Discount
                {
                    Name = addedDiscount.Name,
                    Description = addedDiscount.Description,
                    CreatedAt = DateTime.Now,
                    Percentage = addedDiscount.Percentage,
                    IsActive = addedDiscount.IsActive,
                    ImagePath = addedDiscount.ImagePath,
                    IsDeleted = false
                };

                await _applicationDbContext.Set<Discount>().AddAsync(newDiscount);

                return true;
            }

            return false;
        }
        public override async void Delete(Discount discount)
        {
        //  //  var data = _applicationDbContext.Categories.Where(c => c.Id == discount.Id).FirstOrDefault();
        //  //  data.IsDeleted = !data.IsDeleted;
        //  //await  _applicationDbContext.SaveChangesAsync();
        }
        public override  void Update(Discount updatedDiscount)
        {

            updatedDiscount.ModifiedAt = DateTime.UtcNow;
            _applicationDbContext.Entry(updatedDiscount).State = EntityState.Modified;

        }
        public async Task<List<GetAllDiscountsVM>> GetAllDiscounts()
        {

            var discounts = await _applicationDbContext.Discounts
                                .ToListAsync();

                var discountsVM = _mapper.Map<List<GetAllDiscountsVM>>(discounts);

                return discountsVM;
        }      
        public async Task<DateTime?> ToggleStatus(int id)
        {
          var discount = await _applicationDbContext.Discounts.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (discount != null)
            {
                discount.IsDeleted = !discount.IsDeleted;
                discount.ModifiedAt = DateTime.UtcNow;
                await _applicationDbContext.SaveChangesAsync();
                return DateTime.Now;
            }
            return null;
        }
        public async Task<GetDiscountByIdVM> GetDiscountById(int id)
        {
            var discount = await _applicationDbContext.Discounts
                                .AsNoTracking()
                                .Include(d => d.Products) 
                                .FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null)
            {
                return new GetDiscountByIdVM();
            }

           
            var discountVM = _mapper.Map<GetDiscountByIdVM>(discount);

            return discountVM;
        }
        public async Task<List<GetProductsVM>> GetAllProducts()
        {
            var products = await _applicationDbContext.Products.Where(p => p.DiscountId == null).ToListAsync();
            List<GetProductsVM> productsVM = _mapper.Map<IEnumerable<GetProductsVM>>(products).ToList();
            return productsVM; 
        }
        public async Task<bool> ToggleDiscountAssociation(int productId, int? discountId)
        {
           
            var product = await _applicationDbContext.Products.FindAsync(productId);

            if (product != null)
            {        
                product.DiscountId = discountId;
                return await _applicationDbContext.SaveChangesAsync() > 0;
            }

            return false;
        }



    }



}

