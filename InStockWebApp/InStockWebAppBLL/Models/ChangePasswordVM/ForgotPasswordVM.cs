using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.ChangePasswordVM
{
    public class ForgotPasswordVM
    {
        [Required]
        public string Email { get; set; }
    }
}
