namespace DataAccess.Entities
{
    public class DigitalItems : Items
    {
        public string Color { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? StoreProvider { get; set; } = string.Empty;
        public List<TransactionHistory> TransactionHistories { get; set; }
        public List<LootBoxDigitalItem> LootBoxDigitalItems { get; set; } = new List<LootBoxDigitalItem>();
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}
