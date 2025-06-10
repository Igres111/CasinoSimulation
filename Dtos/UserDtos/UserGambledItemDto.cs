using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.UserDtos
{
    public class UserGambledItemDto
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? StoreProvider { get; set; } = string.Empty;
        public decimal SellPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
