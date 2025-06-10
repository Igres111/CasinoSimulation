namespace DataAccess.Entities
{
    public class TransactionHistory : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ItemId { get; set; }
        public DigitalItems Item { get; set; }
        public decimal Price { get; set; }
    }
}
