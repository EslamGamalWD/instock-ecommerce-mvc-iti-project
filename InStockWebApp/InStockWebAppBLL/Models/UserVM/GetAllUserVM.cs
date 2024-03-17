using InStockWebAppDAL.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Models.UserVM
{
    public record GetAllUserVM(string Id, string Photo, string FirstName, string LastName, UserType UserType, Gender Gender, string CityName, string StateName,DateTime CreatedAt, DateTime? ModifiedAt,bool IsDeleted)
    {
        public GetAllUserVM() : this("",null, "", "", default, default, "", "", DateTime.MinValue, null, false) { }
    }
}
