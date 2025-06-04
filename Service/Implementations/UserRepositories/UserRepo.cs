using DataAccess.Context;
using DataAccess.Entities;
using Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;
using Service.AuthToken;
using Service.Common;
using Service.Interfaces.TokenInterfaces;
using Service.Interfaces.UserInterfaces;

namespace Service.Implementations.UserRepositories
{
    public class UserRepo : APIResponse<string>, IUser
    {
        #region Fields
        public readonly AppDbContext _context;
        public readonly IToken _tokenLogic;
        #endregion

        #region Constructor
        public UserRepo(AppDbContext context, IToken tokenLogic)
        {
            _context = context;
            _tokenLogic = tokenLogic;
        }
        #endregion

        #region Methods

        public async Task<APIResponse<string>> CreateUser(CreateUserDto userInfo)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.Email == userInfo.Email && u.Delete == null);
            if (existUser != null)
            {
               return new APIResponse<string> { Success = false, Error = "User already exists" };
            }
            var newUser = new User
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Email = userInfo.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userInfo.Password),
                CreatedAt = DateTime.UtcNow,
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return new APIResponse<string> { Success = true };
        }

        public async Task<APIResponse<string>> LogInUser(LogInUserDto userInfo)
        {
            var userExists = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == userInfo.Email);
            if (userExists == null)
            {
                return new APIResponse<string> { Success = false, Error = "User does not exist" };
            }
            var passwordMatch = BCrypt.Net.BCrypt.Verify(userInfo.Password, userExists.Password);
            if (!passwordMatch)
            {
                return new APIResponse<string> { Success = false, Error = "Password is incorrect" };
            }
            var refreshToken = await _tokenLogic.CreateRefreshTokenAsync(userExists);
            var accessToken = _tokenLogic.CreateAccessToken(userExists);
            await _context.SaveChangesAsync();
            return new APIResponse<string> { Success = true, Data = accessToken };
        }

        #endregion
    }
}
