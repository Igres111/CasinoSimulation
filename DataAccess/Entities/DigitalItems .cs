using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class DigitalItems : Items
    {
        public string Color { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? StoreProvider { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public List<TransactionHistory> TransactionHistories { get; set; }
    }
}
