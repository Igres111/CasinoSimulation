using DataAccess.Context;
using DataAccess.Entities;
using Dtos.DigitalItemDto;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.Interfaces.DigitalItemsInterfaces;

namespace Service.Implementations.DigitalItemsRepositories
{
    public class DigitalItemsRepo : IDigitalItems
    {
        #region Fields
        public readonly AppDbContext _context;
        #endregion

        #region Constructor
        public DigitalItemsRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task<APIResponse<string>> CreateDigitalItem(CreateDigitalItemsDto digitalItem)
        {
            var existItem = await _context.DigitalItems.FirstOrDefaultAsync(x => x.Name == digitalItem.Name && x.Delete == null);
            if (existItem != null)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Error = "Digital item already exists."
                };
            }
            var newDigitalItem = new DigitalItems
            {
                Name = digitalItem.Name,
                Description = digitalItem.Description,
                Category = digitalItem.Category,
                ImageUrl = digitalItem.ImageUrl,
                SellPrice = digitalItem.SellPrice,
                Color = digitalItem.Color,
                Rarity = digitalItem.Rarity,
                RNG_Ratio = digitalItem.RNG_Ratio,
                Code = digitalItem.Code,
                StoreProvider = digitalItem.StoreProvider,
                CreatedAt = DateTime.UtcNow,
            };
            await _context.DigitalItems.AddAsync(newDigitalItem);
            await _context.SaveChangesAsync();
            return new APIResponse<string>
            {
                Success = true,
                Data = "Digital item created successfully."
            };
        }

        public async Task<APIResponse<List<DigitalItems>>> GetAllDigitalItems()
        {
            var items = await _context.DigitalItems.Where(x => x.Delete == null).ToListAsync();
            if (items == null || items.Count == 0)
            {
                return new APIResponse<List<DigitalItems>> { Success = false, Error = "No digital items found." };
            }
            return new APIResponse<List<DigitalItems>> { Success = true, Data = items };
        }

        public async Task<APIResponse<string>> UpdateItem(UpdateItemDto updateItemDto)
        {
            var item = await _context.DigitalItems.FirstOrDefaultAsync(x => x.Id == updateItemDto.ItemId && x.Delete == null);
            if (item == null)
            {
                return new APIResponse<string> { Success = false, Error = "Digital item not found." };
            }
            item.Name = string.IsNullOrEmpty(updateItemDto.Name) ? item.Name : updateItemDto.Name;
            item.Description = updateItemDto.Description ?? item.Description;
            item.RNG_Ratio = updateItemDto.RNG_Ratio == 0 ? item.RNG_Ratio : updateItemDto.RNG_Ratio;
            item.Category = string.IsNullOrEmpty(updateItemDto.Category) ? item.Category : updateItemDto.Category;
            item.ImageUrl = string.IsNullOrEmpty(updateItemDto.ImageUrl) ? item.ImageUrl : updateItemDto.ImageUrl;
            item.SellPrice = updateItemDto.SellPrice == 0 ? item.SellPrice : updateItemDto.SellPrice;
            item.BonusPoints = updateItemDto.BonusPoints == 0 ? item.BonusPoints : updateItemDto.BonusPoints;
            item.Color = string.IsNullOrEmpty(updateItemDto.Color) ? item.Color : updateItemDto.Color;
            item.Rarity = string.IsNullOrEmpty(updateItemDto.Rarity) ? item.Rarity : updateItemDto.Rarity;
            item.Code = string.IsNullOrEmpty(updateItemDto.Code) ? item.Code : updateItemDto.Code;
            item.StoreProvider = string.IsNullOrEmpty(updateItemDto.StoreProvider) ? item.StoreProvider : updateItemDto.StoreProvider;
            _context.DigitalItems.Update(item);
            await _context.SaveChangesAsync();
            return new APIResponse<string> { Success = true, Data = "Digital item updated successfully." };
        }

        #endregion
    }
}
