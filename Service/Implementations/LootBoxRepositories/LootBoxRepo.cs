using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public readonly AppDbContext _context;
        public LootBoxRepo(AppDbContext context)
        {
            _context = context;
        }
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
            if(lootBoxes.Count == 0)
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
    }
}
