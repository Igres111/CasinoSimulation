using DataAccess.Context;
using DataAccess.Entities;
using Dtos.UserDtos;
using Service.Interfaces.UserInterfaces;

namespace Service.Implementations.UserRepositories
{
    public class UserRepo : IUser
    {
        #region Fields
        public readonly AppDbContext _context;
        #endregion

        #region Constructor
        public UserRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task CreateUser(CreateUserDto userInfo)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.Email == userInfo.Email && u.Delete == null);
            if (existUser != null)
            {
                throw new Exception("User already exists.");
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
        }

        #endregion
    }
}
