using InStockWebAppBLL.Features.Interfaces.Domain;
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
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly ApplicationDbContext db;

        public ContactMessageRepository(ApplicationDbContext db)
        {
            this.db=db;
        }
        public async Task Add(ContactMessage contactMessage)
        {
            await db.ContactMessage.AddAsync(contactMessage);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactMessage>> GetAll()
        {
           return await db.ContactMessage.ToListAsync();
        }

        public async Task<IEnumerable<ContactMessage>> GetBySenderID(string SenderID)
        {
            return await db.ContactMessage.Where(a=>a.UserID == SenderID).ToListAsync();
        }
    }
}
