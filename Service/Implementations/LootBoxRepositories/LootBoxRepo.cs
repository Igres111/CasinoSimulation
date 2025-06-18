using DataAccess.Context;
using DataAccess.Entities;
using Dtos.LootBoxDto;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.Common.LootBoxResponses;
using Service.Interfaces.LootBoxInterfaces;

namespace Service.Implementations.LootBoxRepositories
{
    public class LootBoxRepo : ILootBox
    {
        #region Fields

        public readonly AppDbContext _context;

        #endregion

        #region Constructor

        public LootBoxRepo(AppDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        public async Task<APIResponse<string>> CreateLootBox(CreateLootBoxDto lootInfo)
        {
            var existItem = _context.DigitalItems.Any(x => lootInfo.DigitalItemId.Contains(x.Id));

            if (!existItem)
            {
                return new APIResponse<string> { Success = false, Error = "Digital item does not exist." };
            }
            var existLootBox = await _context.LootBoxes.FirstOrDefaultAsync(x => x.Name == lootInfo.Name && x.Delete == null);

            if (existLootBox != null)
            {
                return new APIResponse<string> { Success = false, Error = "LootBox already exists." };
            }
            var newLootBox = new LootBox
            {
                Name = lootInfo.Name,
                CreatedAt = DateTime.UtcNow,
                Price = lootInfo.Price,
                Quantity = lootInfo.Quantity,
            };
            foreach (var id in lootInfo.DigitalItemId)
            {
                newLootBox.LootBoxDigitalItems.Add(new LootBoxDigitalItem
                {
                    LootBoxId = newLootBox.Id,
                    DigitalItemId = id
                });
            }
            _context.LootBoxes.Add(newLootBox);
            await _context.SaveChangesAsync();
            return new APIResponse<string>
            {
                Success = true,
                Data = "LootBox created successfully."
            };
        }

        public async Task<AllLootBoxResponse> GetAllLootBox()
        {
            var lootBoxes = await _context.LootBoxes
                .Where(x => x.Delete == null)
                .Select(x => new GetAllLootBoxDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                }).ToListAsync();
            if (lootBoxes.Count == 0)
            {
                return new AllLootBoxResponse { Success = false, Error = "No loot boxes found." };
            }
            return new AllLootBoxResponse
            {
                Success = true,
                LootBoxes = lootBoxes,
            };
        }

        public async Task<LootBoxItemsResponse> GetLootBoxItems(Guid lootBoxId)
        {
            var lootBox = await _context.LootBoxDigitalItems
                .Where(el => el.LootBoxId == lootBoxId && el.LootBox.Delete == null)
                .ToListAsync();
            if (lootBox.Count == 0)
            {
                return new LootBoxItemsResponse { Success = false, Error = "No items found in this loot box." };
            }
            return new LootBoxItemsResponse
            {
                Success = true,
                LootBoxItems = lootBox
            };
        }

        public async Task<APIResponse<string>> UpdateLootBox(UpdateLootBox updateInfo)
        {
            var lootBoxExist = await _context.LootBoxes.FirstOrDefaultAsync(x => x.Id == updateInfo.LootBoxId && x.Delete == null);
            if (lootBoxExist == null)
            {
                return new APIResponse<string> { Success = false, Error = "LootBox does not exist." };
            }
            var digitalItemExists = await _context.DigitalItems.AnyAsync(x => x.Id == updateInfo.NewItemId && x.Delete == null);
            if (!digitalItemExists)
            {
                return new APIResponse<string> { Success = false, Error = "Digital item does not exist." };
            }
            if (!string.IsNullOrEmpty(updateInfo.Name))
            {
                lootBoxExist.Name = updateInfo.Name;
            }
            lootBoxExist.Price = updateInfo.Price == 0 ? lootBoxExist.Price : updateInfo.Price;
            lootBoxExist.Quantity = updateInfo.Quantity == 0 ? lootBoxExist.Quantity : updateInfo.Quantity;
            lootBoxExist.UpdatedAt = DateTime.UtcNow;
            if (updateInfo.Swap == true)
            {
                var instanceToChange = await _context.LootBoxDigitalItems
                    .FirstOrDefaultAsync(x => x.DigitalItemId == updateInfo.OldItemId && x.LootBoxId == updateInfo.LootBoxId);
                if (instanceToChange != null)
                {
                    _context.LootBoxDigitalItems.Remove(instanceToChange);
                    await _context.SaveChangesAsync();

                    instanceToChange.DigitalItemId = updateInfo.NewItemId;
                    _context.LootBoxDigitalItems.Add(instanceToChange);
                }
                else
                {
                    return new APIResponse<string> { Success = false, Error = "Old item not found in the LootBox." };
                }
            }
            else
            {
                var itemExists = await _context.LootBoxDigitalItems
                    .AnyAsync(x => x.DigitalItemId == updateInfo.NewItemId && x.LootBoxId == updateInfo.LootBoxId);
                if (!itemExists)
                {

                    var newLootBoxDigitalItem = new LootBoxDigitalItem
                    {
                        LootBoxId = lootBoxExist.Id,
                        DigitalItemId = updateInfo.NewItemId
                    };
                    _context.LootBoxDigitalItems.Add(newLootBoxDigitalItem);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new APIResponse<string> { Success = false, Error = "Item already exists in the loot box." };
                }

            }
            return new APIResponse<string>
            {
                Success = true,
                Data = "LootBox updated successfully."
            };
        }

        public async Task<APIResponse<string>> DeleteLootBox(Guid lootBoxId)
        {
            var lootBoxExist = await _context.LootBoxes.FirstOrDefaultAsync(x => x.Id == lootBoxId && x.Delete == null);
            if (lootBoxExist == null)
            {
                return new APIResponse<string> { Success = false, Error = "LootBox does not exist." };
            }
            lootBoxExist.Delete = DateTime.UtcNow;
            _context.LootBoxes.Update(lootBoxExist);
            await _context.SaveChangesAsync();
            return new APIResponse<string>
            {
                Success = true,
                Data = "LootBox deleted successfully."
            };
        }
        #endregion
    }
}
