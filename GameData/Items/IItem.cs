namespace GameData
{
    public interface IItem
    {
        string Name { get; }
        string Description { get; }
        GoldCoin Amount { get; }
        ItemRarity Rarity { get; }

    }
}
