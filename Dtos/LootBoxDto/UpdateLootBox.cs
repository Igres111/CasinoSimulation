using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.LootBoxDto
{
    public class UpdateLootBox
    {
        public Guid LootBoxId { get; set; }
        public Guid OldItemId { get; set; }
        public Guid NewItemId { get; set; }
        public bool Swap { get; set; } = false; // If true, swap the old item with the new item
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
