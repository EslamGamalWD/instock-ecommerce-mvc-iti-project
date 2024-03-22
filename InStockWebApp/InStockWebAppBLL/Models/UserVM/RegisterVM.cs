using InStockWebAppDAL.Entities.Enumerators;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace InStockWebAppBLL.Models.UserVM
{
    public class RegisterVM
    {
        [Required, MaxLength(20, ErrorMessage = "ERROR : Max Length 20")]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(20, ErrorMessage = "ERROR : Max Length 20!")]
        [MinLength(2, ErrorMessage = "MinLength 2")]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(20, ErrorMessage = "ERROR : Max Length 20!")]
        public string UserName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Password required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email Address required!")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Format!")]
        public string Email { get; set; }

        [Required]
        public int CityId { get; set; }
    }
}
