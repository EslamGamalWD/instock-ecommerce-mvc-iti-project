using InStockWebAppDAL.Entities.Enumerators;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.UserVM
{
    public class UserCheckoutDetailsVM
    {
        public string Id { get; set; }
        [Required, MaxLength(20, ErrorMessage = "Error : Max Length 20")]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(20, ErrorMessage = "Error : Max Length 20")]
        [MinLength(2, ErrorMessage = "MinLength 2")]
        public string LastName { get; set; } = string.Empty;

        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        [Required]
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }

        [Required]
        public int CityId { get; set; }
        
    }
}
