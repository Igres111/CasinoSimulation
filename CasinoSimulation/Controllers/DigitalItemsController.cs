using Dtos.DigitalItemDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.DigitalItemsInterfaces;

namespace CasinoSimulation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigitalItemsController : ControllerBase
    {
        #region Fields
        public readonly IDigitalItems _digitalMethods;
        #endregion

        #region Constructor
        public DigitalItemsController(IDigitalItems digitalMethods)
        {
            _digitalMethods = digitalMethods;
        }
        #endregion

        #region Methods

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllDigitalItems()
        {
            var items = await _digitalMethods.GetAllDigitalItems();
            return Ok(items);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateDigitalItem(CreateDigitalItemsDto digitalItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _digitalMethods.CreateDigitalItem(digitalItem);
            return Ok(result);
        }

        #endregion
    }
}
