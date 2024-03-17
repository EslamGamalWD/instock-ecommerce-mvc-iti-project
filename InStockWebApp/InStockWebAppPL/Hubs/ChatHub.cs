using InStockWebAppDAL.Context;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace InStockWebAppPL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public ChatHub(ApplicationDbContext _context)
        {
            this._context = _context;
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

        public async Task Sendreview( string message,string rating)
        {
            try
            {
                await Clients.All.SendAsync("Receiveview", message, rating);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                await Clients.Caller.SendAsync("FormSubmissionFailed", ex.Message);
            }
        }
    }
}