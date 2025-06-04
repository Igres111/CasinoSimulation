using Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.UserInterfaces;

namespace CasinoSimulation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Fields
        public readonly IUser _userMethods;
        #endregion

        #region Constructor
        public UserController(IUser userMethods)
        {
            _userMethods = userMethods;
        }
        #endregion

        #region Methods

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser(CreateUserDto userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Wrong credentials");
            }
            var result = await _userMethods.CreateUser(userInfo);
            return Ok(result);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogInUser(LogInUserDto userInfo)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Wrong credentials");
            }
            var result = await _userMethods.LogInUser(userInfo);
            return Ok(result);
        }
        #endregion
    }
}
