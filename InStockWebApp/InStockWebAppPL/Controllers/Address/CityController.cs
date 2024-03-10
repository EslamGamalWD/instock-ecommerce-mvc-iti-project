using InStockWebAppBLL.Features.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers.Address
{
    public class CityController : Controller
    {

        #region Prop
        private readonly ICityRepository cityRepository;

        #endregion


        #region Ctor
        public CityController(ICityRepository cityRepository)
        {
            this.cityRepository=cityRepository;
        }
        #endregion


        #region Method
        public async Task<IActionResult> GetAll(int id)
        {
            return Ok(await cityRepository.GetAll(a => a.StateId==id));
        }
        #endregion



    }
}
