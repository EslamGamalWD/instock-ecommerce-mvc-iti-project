using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppDAL.Entities
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string ?FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Photo { get; set; }
        [ForeignKey("User")]
        public string? UserID { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set;}
        public string? Message { get; set; }
        public User ?User { get; set; }
        public string? ReceiverId { get; set; }


    }
}
