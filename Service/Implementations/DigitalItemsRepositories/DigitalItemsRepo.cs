using DataAccess.Context;
using DataAccess.Entities;
using Dtos.DigitalItemDto;
using Microsoft.EntityFrameworkCore;
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

        public async Task CreateDigitalItem(CreateDigitalItemsDto digitalItem)
        {
            var existItem = await _context.DigitalItems.FirstOrDefaultAsync(x => x.Name == digitalItem.Name && x.Delete == null);
            if (existItem != null)
            {
                throw new Exception("Digital item already exists.");
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
        }

        public async Task<List<DigitalItems>> GetAllDigitalItems()
        {
            var items = await _context.DigitalItems.Where(x => x.Delete == null).ToListAsync();
            if (items == null || items.Count == 0)
            {
                throw new Exception("No digital items found.");
            }
            return items;
        }

        #endregion
    }
}
