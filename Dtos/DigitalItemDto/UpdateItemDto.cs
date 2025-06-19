using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.DigitalItemDto
{
    public class UpdateItemDto
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal RNG_Ratio { get; set; } = 0.1m;
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal SellPrice { get; set; }
        public int BonusPoints { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? StoreProvider { get; set; } = string.Empty;
    }
}
