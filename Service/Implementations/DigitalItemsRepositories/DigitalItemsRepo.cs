using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Entities;
using Dtos.DigitalItemDto;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.DigitalItemsInterfaces;

namespace Service.Implementations.DigitalItemsRepositories
{
    public class DigitalItemsRepo:IDigitalItems
    {
        public readonly AppDbContext _context;
        public DigitalItemsRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateDigitalItem(CreateDigitalItemsDto digitalItem)
        {
            var existItem = await _context.DigitalItems.FirstOrDefaultAsync(x => x.Name == digitalItem.Name);
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
    }
}
