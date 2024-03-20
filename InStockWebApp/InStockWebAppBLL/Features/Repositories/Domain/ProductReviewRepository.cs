using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ReviewVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ApplicationDbContext db;
       
        private readonly IMapper mapper;
        public ProductReviewRepository(ApplicationDbContext db, IMapper mapper)
        {
            this.mapper = mapper;
            this.db = db;
        }
        public async Task Add(ReviewVM review)
        {
            var Result =  mapper.Map<ProductReview>(review);
            await db.ProductReviews.AddAsync(Result);
            await db.SaveChangesAsync();
        }

        public async Task CalculateAverageRating(int id)
        {
            var Rating = await db.ProductReviews.Where(a => a.ProductId==id).ToListAsync();


            int sum = 0;
            foreach (var rating in Rating)
            {
                sum += rating.Rating;
            }

            decimal calcrating = (decimal)sum /(decimal)Rating.Count;

            var Product = await db.Products.Where(a=>a.Id == id).FirstOrDefaultAsync();
            Product.AvgRating = calcrating;
            await db.SaveChangesAsync();
        }
    }
}
