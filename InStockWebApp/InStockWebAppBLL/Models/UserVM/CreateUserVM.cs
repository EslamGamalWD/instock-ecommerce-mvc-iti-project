using InStockWebAppDAL.Entities.Enumerators;
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

        public string LastName { get; set; } = string.Empty;
        [Required]

        public UserType UserType { get; set; }
        [Required]

        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]

        public int CityId { get; set; }
    }
}
