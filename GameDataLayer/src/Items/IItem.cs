namespace GameDataLayer
{
    public interface IItem
    {
        string Name { get; }
        string Description { get; }
        GoldCoin Amount { get; }
    }
}
