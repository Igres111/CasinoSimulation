using Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.AuthToken;
using Service.Interfaces.TokenInterfaces;
using Service.Interfaces.UserInterfaces;

namespace CasinoSimulation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Fields
        public readonly IUser _userMethods;
        public readonly IToken _tokenLogic;
        #endregion

        #region Constructor
        public UserController(IUser userMethods, IToken tokenLogic)
        {
            _userMethods = userMethods;
            _tokenLogic = tokenLogic;
        }
        #endregion

        #region Methods

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser(CreateUserDto userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userMethods.CreateUser(userInfo);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogInUser(LogInUserDto userInfo)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userMethods.LogInUser(userInfo);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpPost("Refresh-access-token")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            var result = await _tokenLogic.RefreshAccessTokenAsync(token);
            return Ok(result);
        }

        [HttpPost("Gamble")]
        public async Task<IActionResult> Gamble(UserGambleDto betInfo)
        {
            var result = await _userMethods.UserGamble(betInfo);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpPost("SellItem")]
        public async Task<IActionResult> SellItem(SellItemDto itemInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userMethods.SellItem(itemInfo);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> UserProfile(Guid Id)
        {
            var result = await _userMethods.UserProfile(Id);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpGet("Inventory")]
        public async Task<IActionResult> UserInventory(Guid Id)
        {
            var result = await _userMethods.UserInventory(Id);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        #endregion
    }
}
