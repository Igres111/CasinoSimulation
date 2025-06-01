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
        public readonly IUser _userMethods;
        public UserController(IUser userMethods)
        {
            _userMethods = userMethods;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserDto userInfo)
        {
            if (!ModelState.IsValid)
            {
              return BadRequest("Wrong credentials");
            }
            await _userMethods.CreateUser(userInfo);
            return Ok("User created successfully");
        }
    }
}
