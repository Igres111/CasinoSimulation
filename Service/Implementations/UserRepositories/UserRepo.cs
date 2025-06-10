using DataAccess.Context;
using DataAccess.Entities;
using Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;
using Service.AuthToken;
using Service.Common;
using Service.Common.UserResponses;
using Service.Interfaces.TokenInterfaces;
using Service.Interfaces.UserInterfaces;

namespace Service.Implementations.UserRepositories
{
    public class UserRepo : IUser
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

        public async Task<GambledItemResponse> UserGamble(UserGambleDto betInfo)
        {
            var userExists = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == betInfo.UserId && x.Delete == null);
            var lootBoxExists = await _context.LootBoxes
                .AsNoTracking()
                .Include(x => x.LootBoxDigitalItems)
                .ThenInclude(x => x.DigitalItem)
                .FirstOrDefaultAsync(x => x.Id == betInfo.LootBoxId && x.Delete == null);

            if (userExists == null || lootBoxExists == null)
            {
                return new GambledItemResponse { Success = false, Error = "User or LootBox doesn't Exist" };
            }

            if(userExists.Balance < lootBoxExists.Price)
            {
                return new GambledItemResponse { Success = false, Error = "User is out of balance" };
            }

            if (lootBoxExists.Quantity <= 0)
            {
                return new GambledItemResponse { Success = false, Error = "LootBox is out of stock" };
            }

            var totalRatio = lootBoxExists.LootBoxDigitalItems.Sum(x => x.DigitalItem.RNG_Ratio);
            Random rng = new Random();
            var roll = (decimal)rng.NextDouble() * totalRatio;
            decimal cumulative = 0;
            foreach (var item in lootBoxExists.LootBoxDigitalItems)
            {
                cumulative += item.DigitalItem.RNG_Ratio;
                if (roll < cumulative)
                {
                    var addInInventory = new Inventory
                    {
                        UserId = userExists.Id,
                        DigitalItemId = item.DigitalItemId,
                        CreatedAt = DateTime.UtcNow,
                        Quantity = 1
                    };
                    var transactionHistory = new TransactionHistory
                    {
                        UserId = userExists.Id,
                        ItemId = item.DigitalItemId,
                        CreatedAt = DateTime.UtcNow,
                        Price = lootBoxExists.Price,
                    };
                    _context.Inventories.Add(addInInventory);
                    _context.TransactionHistories.Add(transactionHistory);
                    await _context.SaveChangesAsync();
                    return new GambledItemResponse { Success= true, DigitalItem = new UserGambledItemDto 
                    {
                        ItemId = item.DigitalItem.Id,
                        Name = item.DigitalItem.Name,
                        Description = item.DigitalItem.Description,
                        ImageUrl = item.DigitalItem.ImageUrl,
                        Category = item.DigitalItem.Category,
                        SellPrice = item.DigitalItem.SellPrice,
                        Color = item.DigitalItem.Color,
                        Rarity = item.DigitalItem.Rarity,
                        Code = item.DigitalItem.Code,
                        StoreProvider = item.DigitalItem.StoreProvider,
                        UserId = userExists.Id
                    },
                    } ;
                }
            }
            return new GambledItemResponse { Success = false, Error = "No item won. Something went wrong" };
        }

        #endregion
    }
}
