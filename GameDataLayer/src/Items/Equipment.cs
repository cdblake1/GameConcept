using GameDataLayer;

public class Equipment : IItem
{
    public string Name { get; init; }
    public string Description { get; init; }
    public StatTemplate Stats { get; init; }
    public EquipmentKind Kind { get; init; }

    public GoldCoin Amount { get; init; }

    public Equipment(string name, string description, GoldCoin amount, StatTemplate stats, EquipmentKind kind)
    {
        Name = name;
        Description = description;
        Stats = stats;
        Kind = kind;
        Amount = amount;
    }

    public override string ToString()
    {
        return $"{Name}: {Description} [Kind: {Kind}] (Stats: AttackPower={Stats.AttackPower}, Defense={Stats.Defense}, Health={Stats.Health})";
    }
}