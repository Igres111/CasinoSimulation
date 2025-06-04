using DataAccess.Entities;

public class LootBoxDigitalItem
{
    public Guid LootBoxId { get; set; }
    public LootBox LootBox { get; set; }

    public Guid DigitalItemId { get; set; }
    public DigitalItems DigitalItem { get; set; }
}

