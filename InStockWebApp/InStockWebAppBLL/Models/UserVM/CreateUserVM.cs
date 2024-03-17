using InStockWebAppDAL.Entities;
using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.UserVM
{
    public class CreateUserVM
    {
        
        [Required,MaxLength(20,ErrorMessage ="Error : Max Length 20")]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(20, ErrorMessage = "Error : Max Length 20")]
        [MinLength(2,ErrorMessage ="MinLength 2")]
        public string LastName { get; set; } = string.Empty;
        [Required]

        public UserType UserType { get; set; }
        [Required]

        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "UserName required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string PasswordHash { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public IFormFile? image { get; set; }
        public string? Photo { get; set; }
        [Required]

        public int CityId { get; set; }
        public virtual IEnumerable<UserPayment>? UserPaymentVM { get; set; }
    }
}
