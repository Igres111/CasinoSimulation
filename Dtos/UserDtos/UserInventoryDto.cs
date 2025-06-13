using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.UserDtos
{
    public class UserInventoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal SellPrice { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? StoreProvider { get; set; } = string.Empty;
    }
}
