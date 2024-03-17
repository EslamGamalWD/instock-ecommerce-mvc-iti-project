using Hangfire;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ReviewVM;
using InStockWebAppDAL.Context;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace InStockWebAppPL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IProductReviewRepository productReviewRepository;
        public ChatHub(IProductReviewRepository productReviewRepository)
        {
            this.productReviewRepository = productReviewRepository;
        }
        public async Task SendFormToAdmin(string firstName, string lastName, string email, string phone, string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveFormFromUser", firstName, lastName, email, phone, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                await Clients.Caller.SendAsync("FormSubmissionFailed", ex.Message);
            }
        }

        public async Task Sendreview( string message,string rating,string image,string name,string productId,string userId)
        {
            try
            {
                ReviewVM review = new ReviewVM()
                {
                    CreatedAt = DateTime.Now,
                    ProductId =int.Parse(productId),
                    UserId =userId,
                    Rating =int.Parse(rating),
                    Review = message,

                };
                BackgroundJob.Enqueue( () => productReviewRepository.Add(review));
                BackgroundJob.Enqueue(() => productReviewRepository.CalculateAverageRating(int.Parse(productId)));

                await Clients.All.SendAsync("Receiveview", message, rating, image, name, productId, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                await Clients.Caller.SendAsync("FormSubmissionFailed", ex.Message);
            }
        }
    }
}