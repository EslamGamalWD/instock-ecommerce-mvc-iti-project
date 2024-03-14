using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.ImageUploader;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ProductImageRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> add(IEnumerable<IFormFile> images, int productId)
        {
            try
            {
               foreach (var image in images)
                {
                    ProductImage newImage=new();
                    if (image != null)
                    {
                        // Check if the image path already exists in the database
                        var existingImage = await _applicationDbContext.Images.FirstOrDefaultAsync(i => i.ImagePath == image.FileName);
                        if(existingImage != null&& existingImage.ProductId==productId) { continue; }
                        if(existingImage==null) newImage.ImagePath = FilesUploader.UploadFile("ProductImages", image);
                        else newImage.ImagePath = image.FileName;
                        newImage.ProductId=productId;
                        await _applicationDbContext.Images.AddAsync(newImage);
                        await _applicationDbContext.SaveChangesAsync();
                       
                    }
                }
               return true;
            }catch(Exception) 
                {
                return false; 
            }
        }
    }
}
