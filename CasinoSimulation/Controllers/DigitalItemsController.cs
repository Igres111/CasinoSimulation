using Dtos.DigitalItemDto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.DigitalItemsInterfaces;
using Sprache;

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

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllDigitalItems()
        {
            var items = await _digitalMethods.GetAllDigitalItems();
            return Ok(items);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateDigitalItem(CreateDigitalItemsDto digitalItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _digitalMethods.CreateDigitalItem(digitalItem);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateItem(UpdateItemDto itemInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _digitalMethods.UpdateItem(itemInfo);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            var result = await _digitalMethods.DeleteItem(itemId);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
        #endregion
    }
}
