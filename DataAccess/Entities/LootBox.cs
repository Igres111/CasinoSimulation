using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class LootBox
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; } 
        public List<LootBoxDigitalItem> LootBoxDigitalItems { get; set; } = new List<LootBoxDigitalItem>();
    }
}
