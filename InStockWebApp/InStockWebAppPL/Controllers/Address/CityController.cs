using InStockWebAppBLL.Features.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers.Address
{
    public class CityController : Controller
    {
        private readonly ICityRepository cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            this.cityRepository=cityRepository;
        }
        public async Task< IActionResult> GetAll(int id )
        {
            return Ok(await cityRepository.GetAll(a=>a.StateId==id));
        }
    }
}
