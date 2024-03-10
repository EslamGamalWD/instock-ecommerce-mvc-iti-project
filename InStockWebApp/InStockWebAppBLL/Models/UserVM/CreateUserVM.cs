using InStockWebAppDAL.Entities.Enumerators;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace InStockWebAppBLL.Models.UserVM
{
    public class CreateUserVM
    {

        [Required,MaxLength(20,ErrorMessage ="Error : Max Length 20")]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(20, ErrorMessage = "Error : Max Length 20")]

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
     
        public string? Email { get; set; }
        public IFormFile? image { get; set; }
        public byte[]? Photo { get; set; }
        [Required]

        public int CityId { get; set; }
    }
}
