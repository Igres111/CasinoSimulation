using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User: BaseEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        string? PreferredLanguage { get; set; }
        string? AvatarUrl { get; set; }
        public int TotalBoxesOpened { get; set; }
        public decimal Balance { get; set; }
        public int BonusPoints { get; set; }
        public List<DigitalItems> DigitalItems { get; set; }
        public List<TransactionHistory> TransactionHistories { get; set; }
    }
}
