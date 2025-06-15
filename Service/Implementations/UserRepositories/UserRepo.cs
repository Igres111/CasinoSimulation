using DataAccess.Context;
using DataAccess.Entities;
using Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.Common;
using Service.Common.LootBoxResponses;
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
                .FirstOrDefaultAsync(x => x.Id == betInfo.UserId && x.Delete == null);
            var lootBoxExists = await _context.LootBoxes
                .Include(x => x.LootBoxDigitalItems)
                .ThenInclude(x => x.DigitalItem)
                .FirstOrDefaultAsync(x => x.Id == betInfo.LootBoxId && x.Delete == null);

            if (userExists == null || lootBoxExists == null)
            {
                return new GambledItemResponse { Success = false, Error = "User or LootBox doesn't Exist" };
            }

            if (userExists.Balance < lootBoxExists.Price)
            {
                return new GambledItemResponse { Success = false, Error = "User is out of balance" };
            }

            if (lootBoxExists.Quantity <= 0)
            {
                return new GambledItemResponse { Success = false, Error = "LootBox is out of stock" };
            }

            using var transaction = await _context.Database.BeginTransactionAsync();


            var totalRatio = lootBoxExists.LootBoxDigitalItems.Sum(x => x.DigitalItem.RNG_Ratio);
            Random rng = new Random();
            var roll = (decimal)rng.NextDouble() * totalRatio;
            decimal cumulative = 0;
            try
            {
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
                        userExists.BonusPoints += item.DigitalItem.BonusPoints;
                        userExists.Balance -= lootBoxExists.Price;
                        userExists.TotalBoxesOpened += 1;
                        lootBoxExists.Quantity -= 1;
                        _context.Users.Update(userExists);
                        _context.LootBoxes.Update(lootBoxExists);
                        var itemExists = await _context.Inventories
                            .FirstOrDefaultAsync(x => x.UserId == userExists.Id && x.DigitalItemId == item.DigitalItemId && x.Delete == null);
                        if (itemExists == null)
                        {
                            _context.Inventories.Add(addInInventory);
                        }
                        else
                        {
                            itemExists.Quantity += betInfo.Quantity;
                        }
                        _context.TransactionHistories.Add(transactionHistory);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new GambledItemResponse
                        {
                            Success = true,
                            DigitalItem = new UserGambledItemDto
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
                        };
                    }
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                return new GambledItemResponse
                {
                    Success = false,
                    Error = "An error occurred during the transaction."

                };
            }
            return new GambledItemResponse { Success = false, Error = "No item won. Something went wrong" };
        }

        public async Task<UserProfileResponse> UserProfile(Guid UserId)
        {
            var userExist = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId && x.Delete == null);
            if (userExist == null)
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Error = "User does not exist"
                };
            }
            ;

            var userProfile = new UserProfileDto
            {
                Id = userExist.Id,
                FirstName = userExist.FirstName,
                LastName = userExist.LastName,
                Balance = userExist.Balance,
                BonusPoints = userExist.BonusPoints,
                PreferredLanguage = userExist.PreferredLanguage,
                AvatarUrl = userExist.AvatarUrl,
                TotalBoxesOpened = userExist.TotalBoxesOpened
            };
            return new UserProfileResponse { Success = true, UserProfile = userProfile };
        }
        public async Task<InventoryItemsResponse> UserInventory(Guid UserId)
        {
            var userExist = await _context.Users
                .Where(x => x.Id == UserId && x.Delete == null)
                .Include(i => i.Inventories)
                .ThenInclude(i => i.DigitalItem)
                .FirstOrDefaultAsync(x => x.Id == UserId && x.Delete == null);
            if (userExist == null)
            {
                return new InventoryItemsResponse
                {
                    Success = false,
                    Error = "User does not exist"
                };
            }

            var inventoryItems = userExist.Inventories
                .Where(x => x.Delete == null && x.Quantity > 0)
                .SelectMany(item => Enumerable.Range(0, item.Quantity)
                    .Select(_ => new UserInventoryDto
                    {
                        Id = item.DigitalItem.Id,
                        Name = item.DigitalItem.Name,
                        Description = item.DigitalItem.Description,
                        Category = item.DigitalItem.Category,
                        ImageUrl = item.DigitalItem.ImageUrl,
                        SellPrice = item.DigitalItem.SellPrice,
                        Color = item.DigitalItem.Color,
                        Rarity = item.DigitalItem.Rarity,
                        Code = item.DigitalItem.Code,
                        StoreProvider = item.DigitalItem.StoreProvider,
                    }))
                .ToList();
            if (inventoryItems.Count == 0)
            {
                return new InventoryItemsResponse
                {
                    Success = false,
                    Error = "User inventory is empty"
                };
            }
            return new InventoryItemsResponse
            {
                Success = true,
                Data = "User inventory items retrieved successfully",
                InventoryItems = inventoryItems
            };
        }
        public async Task<APIResponse<string>> SellItem(SellItemDto itemInfo)
        {
            var userExist = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == itemInfo.UserId && x.Delete == null);
            if (userExist == null)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Error = "User does not exist"
                };
            }

            var itemExist = await _context.Inventories
                .Include(x => x.DigitalItem)
                .FirstOrDefaultAsync(u => u.UserId == itemInfo.UserId && u.DigitalItemId == itemInfo.ItemId && u.Delete == null);

            if (itemExist == null)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Error = "Item does not exist in user's inventory"
                };
            }
            if (itemExist.Quantity < itemInfo.Quantity)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Error = "Not enough items to withdraw"
                };
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                itemExist.Quantity -= itemInfo.Quantity;
                userExist.Balance += itemExist.DigitalItem.SellPrice * itemInfo.Quantity;
                if (itemExist.Quantity == 0)
                {
                    itemExist.Delete = DateTime.UtcNow;
                }
                else
                {
                    itemExist.UpdatedAt = DateTime.UtcNow;
                }
                var transHistory = await _context.TransactionHistories
                .AddAsync(new TransactionHistory
                {
                    UserId = userExist.Id,
                    ItemId = itemExist.DigitalItemId,
                    CreatedAt = DateTime.UtcNow,
                    Price = itemExist.DigitalItem.SellPrice * itemInfo.Quantity,
                });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new APIResponse<string>
                {
                    Success = true,
                    Data = "Item withdrawn successfully",
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                return new APIResponse<string>
                {
                    Success = false,
                    Error = "An error occurred during the transaction."
                };
            }
        }
        public async Task<APIResponse<string>> UpdateProfile(UpdateProfileDto userInfo)
        {
            var userExist = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userInfo.UserId && x.Delete == null);
            if (userExist == null)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Error = "User does not exist"
                };
            }

            userExist.FirstName = string.IsNullOrEmpty(userInfo.FirstName) ? userExist.FirstName : userInfo.FirstName;
            userExist.LastName = string.IsNullOrEmpty(userInfo.LastName) ? userExist.LastName : userInfo.LastName;
            userExist.Email = string.IsNullOrEmpty(userInfo.Email) ? userExist.Email : userInfo.Email;
            userExist.PreferredLanguage = string.IsNullOrEmpty(userInfo.PreferredLanguage) ? userExist.PreferredLanguage : userInfo.PreferredLanguage;
            userExist.AvatarUrl = string.IsNullOrEmpty(userInfo.AvatarUrl) ? userExist.AvatarUrl : userInfo.AvatarUrl;
            userExist.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(userExist);
            await _context.SaveChangesAsync();
            return new APIResponse<string>
            {
                Success = true,
                Data = "User profile updated successfully"
            };
        }
        public async Task<APIResponse<string>> DeleteProfile(Guid UserId)
        {
            var userExist = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == UserId && x.Delete == null);
            if (userExist == null)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Error = "User does not exist"
                };
            }
            userExist.Delete = DateTime.UtcNow;
            _context.Users.Update(userExist);
            await _context.SaveChangesAsync();
            return new APIResponse<string>
            {
                Success = true,
                Data = "User profile deleted successfully"
            };
        }
    }
    #endregion
}
