using InStockWebAppDAL.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.UserVM
{
   
        public record GetUserByIdVM(string Id, string FirstName, string LastName, UserType UserType, Gender Gender, string CityName, string StateName,string Email)
        {
            public GetUserByIdVM() : this("", "", "", default, default, "", "","") { }
        }
    
}
