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
        public readonly IDigitalItems _digitalMethods;
        public DigitalItemsController(IDigitalItems digitalMethods)
        {
            _digitalMethods = digitalMethods;
        }

        [HttpPost("CreateDigitalItem")]
        public async Task<IActionResult> CreateDigitalItem(CreateDigitalItemsDto digitalItem)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _digitalMethods.CreateDigitalItem(digitalItem);
            return Ok("Digital item created successfully");
        }
    }
}
