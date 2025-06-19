using Dtos.LootBoxDto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.LootBoxInterfaces;

namespace CasinoSimulation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LootBoxController : ControllerBase
    {
        public readonly ILootBox _lootBoxMethods;
        public LootBoxController(ILootBox lootBoxMethods)
        {
            _lootBoxMethods = lootBoxMethods;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateLootBox(CreateLootBoxDto lootInfo)
        {
            var result = await _lootBoxMethods.CreateLootBox(lootInfo);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLootBox()
        {
            var result = await _lootBoxMethods.GetAllLootBox();
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
        [HttpGet("items/{lootBoxId}")]
        public async Task<IActionResult> GetLootBoxItems(Guid lootBoxId)
        {
            var result = await _lootBoxMethods.GetLootBoxItems(lootBoxId);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateLootBox([FromBody] UpdateLootBox updateInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _lootBoxMethods.UpdateLootBox(updateInfo);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
        [HttpDelete("{lootBoxId}")]
        public async Task<IActionResult> DeleteLootBox(Guid lootBoxId)
        {
            var result = await _lootBoxMethods.DeleteLootBox(lootBoxId);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
    }
}
