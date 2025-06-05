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
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllLootBox()
        {
            var result = await _lootBoxMethods.GetAllLootBox();
            return Ok(result);
        }

    }
}
