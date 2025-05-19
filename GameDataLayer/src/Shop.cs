using GameDataLayer;

public class Shop
{
    public string Name { get; set; }
    public List<IItem> Items { get; set; } = new();

    private readonly double purchasePercent;

    public Shop(string name, List<IItem> items, double purchasePercent = 0.3)
    {
        Name = name;
        Items = items;
        this.purchasePercent = purchasePercent;
    }

    public GoldCoin Sell(IItem item)
    {
        return GoldCoin.FromAmount(item.Amount * purchasePercent);
    }

    public IItem Buy(IItem item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            return item;
        }
        else
        {
            throw new InvalidOperationException("Item not found in shop.");
        }
    }
}