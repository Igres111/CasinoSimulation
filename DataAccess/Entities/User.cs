namespace DataAccess.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PreferredLanguage { get; set; }
        public string? AvatarUrl { get; set; }
        public int TotalBoxesOpened { get; set; }
        public decimal Balance { get; set; }
        public int BonusPoints { get; set; }       
        public List<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<LootBox> LootBoxes { get; set; } = new List<LootBox>();
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}
