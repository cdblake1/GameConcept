using static StatTemplate;

namespace GameData;

public class Equipment : IItem, IStateSerializable<EquipmentDto, Equipment>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public StatTemplate Stats { get; init; }
    public EquipmentKind Kind { get; init; }

    public GoldCoin Amount { get; init; }

    public ItemRarity Rarity { get; set; }

    public Equipment(string name, string description, GoldCoin amount, StatTemplate stats, EquipmentKind kind, ItemRarity rarity)
    {
        Name = name;
        Description = description;
        Stats = stats;
        Kind = kind;
        Amount = amount;
        Rarity = rarity;
    }

    public override string ToString()
    {
        return $"{Name}: {Description} [Kind: {Kind}] (Stats: AttackPower={Stats.AttackPower}, Defense={Stats.Defense}, Health={Stats.Health})";
    }

    public EquipmentDto Serialize()
    {
        return new EquipmentDto
        {
            Name = Name,
            Description = Description,
            Stats = Stats.Serialize(),
            Kind = Kind,
            Amount = Amount.Amount
        };
    }

    public static Equipment Restore(EquipmentDto dto)
    {
        return new Equipment(
            dto.Name,
            dto.Description,
            GoldCoin.FromAmount(dto.Amount),
            StatTemplate.Restore(dto.Stats),
            dto.Kind,
            dto.Rarity);
    }
}

public readonly struct EquipmentDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public StatTemplateDto Stats { get; init; }
    public EquipmentKind Kind { get; init; }
    public int Amount { get; init; }
    public ItemRarity Rarity { get; init; }
}
