using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.ImageUploader;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Hosting;
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

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductImageRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<bool> remove(IEnumerable<ProductImage> images, int productId)
        {
            try
            {
                List<ProductImage> tempImages =[.. images];
                // Check if the image path already exists in the database
                //var existingImage = await _applicationDbContext.Images.FirstOrDefaultAsync;
                _applicationDbContext.Images.RemoveRange(images);
                await _applicationDbContext.SaveChangesAsync();
                foreach (var image in tempImages)
                {
                    if (image != null)
                    {
                        
                        bool needed= await _applicationDbContext.Images.AnyAsync(i => i.ImagePath == image.ImagePath);
                        if(!needed)
                        {
                            string oldImgFullPath = _webHostEnvironment.WebRootPath + "\\Files\\ProductImages\\"+ image.ImagePath;

                            if (File.Exists(oldImgFullPath))
                            {
                               File.Delete(oldImgFullPath);
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
