using GameDataLayer;

public class Shop
{
    public string Name { get; set; }
    public List<IItem> Items { get; set; } = new();


    public Shop(string name, List<IItem> items)
    {
        Name = name;
        Items = items;
    }

    public GoldCoin Sell(IItem item)
    {
        return GoldCoin.FromAmount(item.Amount.Amount);
    }

    public GoldCoin Sell(IItem item, int amount)
    {
        return GoldCoin.FromAmount(item.Amount * amount);
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