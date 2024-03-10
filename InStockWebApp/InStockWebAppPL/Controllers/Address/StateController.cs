using InStockWebAppBLL.Features.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers.Address
{
    public class StateController : Controller
    {
        private readonly IStateRepository stateRepository;

        public StateController(IStateRepository stateRepository)
        {
            this.stateRepository=stateRepository;
        }
        public async Task<IActionResult> GetAll(int id)
        {
            return Ok(await stateRepository.GetAll(a=>a.CountryID == id));
        }
    }
}
