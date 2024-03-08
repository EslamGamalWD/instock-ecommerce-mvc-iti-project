using InStockWebAppDAL.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.UserVM
{
    public record GetAllUserVM(string Id, string FirstName, string LastName, UserType UserType, Gender Gender, string CityName, string StateName)
    {
        public GetAllUserVM() : this("", "", "", default, default, "", "") { }
    }
}
