using Hangfire;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.Account;
using InStockWebAppBLL.Models.ReviewVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Threading.Tasks;

namespace InStockWebAppPL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IContactMessageRepository addContactMessageRepository;
        private readonly IEmailSender emailSender;

        public ChatHub(IProductReviewRepository productReviewRepository, IContactMessageRepository addContactMessageRepository, IEmailSender emailSender)
        {
            this.productReviewRepository = productReviewRepository;
            this.addContactMessageRepository=addContactMessageRepository;
            this.emailSender=emailSender;
        }
        public async Task SendFormToAdmin(string Id,string firstName, string lastName, string email, string phone, string message,string image)
        {
            try
            {
                ContactMessage contactMessage = new ContactMessage() {
                    FirstName= firstName,
                    UserID =Id,
                    LastName=lastName,
                    Email=email,
                    PhoneNumber=phone,
                    Photo=image,
                    Message=message
                };
                BackgroundJob.Enqueue(() => addContactMessageRepository.Add(contactMessage));
                await Clients.All.SendAsync("ReceiveFormFromUser", Id,firstName, lastName, email, phone, message, image);
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


        public async Task SendMessage(string receiverId, string message,string SenderId,string email)
        {

            ContactMessage contactMessage = new ContactMessage()
            {
               
                Message=message,
                UserID =receiverId,
                ReceiverId =SenderId
            };
            BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(email, "Admin Message", "Wellcom in instock admin send To you Message check your Profile Please"));
            BackgroundJob.Enqueue(() => addContactMessageRepository.Add(contactMessage));

            await Clients.User(receiverId).SendAsync("ReceiveMessage", receiverId, message,SenderId, email);
        }
    }
}