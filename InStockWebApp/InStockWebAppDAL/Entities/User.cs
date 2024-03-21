using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Identity;

namespace InStockWebAppDAL.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public UserType UserType { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ModifiedAt { get; set; }
        public string? Photo { get; set; }

        public string? AddressLine { get; set; }

        //Navigation Property
        public int? CityId { get; set; }

        public City City { get; set; } = default!;
        
        public virtual Cart? Cart { get; set; }
        public virtual IEnumerable<UserPayment>? UserPayment { get; set; }

        public virtual ICollection<ContactMessage> ?ContactMessages { get; set; }
    }
}