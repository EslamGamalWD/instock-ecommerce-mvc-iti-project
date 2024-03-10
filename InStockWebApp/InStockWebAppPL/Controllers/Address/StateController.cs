using InStockWebAppBLL.Features.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers.Address
{
    public class StateController : Controller
    {


        #region Prop
        private readonly IStateRepository stateRepository;

        #endregion


        #region Ctor
        public StateController(IStateRepository stateRepository)
        {
            this.stateRepository=stateRepository;
        }
        #endregion


        #region Method
        public async Task<IActionResult> GetAll(int id)
        {
            return Ok(await stateRepository.GetAll(a => a.CountryID == id));
        }
        #endregion



    }
}
