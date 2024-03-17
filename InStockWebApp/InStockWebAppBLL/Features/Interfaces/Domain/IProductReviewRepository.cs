using InStockWebAppBLL.Models.ReviewVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IProductReviewRepository
    {
        Task Add(ReviewVM review);
        Task CalculateAverageRating(int id);
    }
}
