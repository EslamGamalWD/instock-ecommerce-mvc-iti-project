using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IContactMessageRepository
    {
        Task Add(ContactMessage contactMessage);
        Task<IEnumerable<ContactMessage>> GetAll();
        Task<IEnumerable<ContactMessage>> GetBySenderID(string SenderID);
    }
}
