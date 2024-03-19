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
    public class EditUserVM
    {
        public string Id { get; set; }
        [Required, MaxLength(20, ErrorMessage = "Error : Max Length 20")]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(20, ErrorMessage = "Error : Max Length 20")]
        [MinLength(2, ErrorMessage = "MinLength 2")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public Gender Gender { get; set; }
        public IFormFile? Image { get; set; }
        public string? Photo { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? CityId { get; set; }
    }
}
