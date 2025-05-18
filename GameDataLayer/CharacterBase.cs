using System.Security.Cryptography.X509Certificates;

namespace GameDataLayer;

public class CharacterBase
{
    private readonly string name;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; set; }
    public string Name => name;

    private readonly List<Item> inventory = [];
    
    public readonly Equipment Equipment = new Equipment();

    public CharacterBase(string name, StatTemplate baseStats)
    {
        this.baseStats = baseStats;
        MaxHealth = baseStats.Health;
        CurrentHealth = MaxHealth;
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");
        }

        this.name = name;
    }

    public void AddItemToInventory(Item item)
    {
        inventory.Add(item);
    }

    public void DeleteItem(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
        }
        else
        {
            throw new InvalidOperationException("Item not found in inventory.");
        }
    }

    public void EquipItem(Item item)
    {
        if (!inventory.Contains(item))
        {
            throw new InvalidOperationException("Item must be in inventory to equip.");
        }

        inventory.Remove(item);

        switch (item.Kind)
        {
            case ItemKind.Weapon:
                Equipment.Weapon = item;
                break;
            case ItemKind.BodyArmor:
                Equipment.BodyArmor = item;
                break;
            case ItemKind.HeadArmor:
                Equipment.HeadArmor = item;
                break;
            case ItemKind.LegArmor:
                Equipment.LegsArmor = item;
                break;
            default:
                throw new InvalidOperationException("Item cannot be equipped.");
        }
    }

    public void UnequipItem(ItemKind itemKind)
    {
        Item? item = itemKind switch
        {
            ItemKind.Weapon => Equipment.Weapon,
            ItemKind.BodyArmor => Equipment.BodyArmor,
            ItemKind.HeadArmor => Equipment.HeadArmor,
            ItemKind.LegArmor => Equipment.LegsArmor,
            _ => throw new InvalidOperationException("Item cannot be unequipped."),
        };

        if (item is Item equippedItem)
        {
            AddItemToInventory(equippedItem);
        }
    }

    private readonly StatTemplate baseStats;

    public StatTemplate Stats => this.baseStats;

    public double Attack(CharacterBase target)
    {
        double damage = Math.Max(0, this.Stats.AttackPower - target.Stats.Defense);
        target.CurrentHealth = Math.Max(0, target.CurrentHealth - (int)damage);
        return damage;
    }
}

public record struct Item
{
    public string Name { get; init; }
    public string Description { get; init; }
    public StatTemplate Stats { get; init; }
    public ItemKind Kind { get; init; }

    public Item(string name, string description, StatTemplate stats, ItemKind kind)
    {
        Name = name;
        Description = description;
        Stats = stats;
        Kind = kind;
    }

    public override string ToString()
    {
        return $"{Name}: {Description} [Kind: {Kind}] (Stats: AttackPower={Stats.AttackPower}, Defense={Stats.Defense}, Health={Stats.Health})";
    }
}

public class Equipment
{
    public Item? Weapon { get; set; }
    public Item? BodyArmor { get; set; }
    public Item? HeadArmor { get; set; }
    public Item? LegsArmor { get; set; }

    public Equipment()
    {
        Weapon = null;
        BodyArmor = null;
        HeadArmor = null;
        LegsArmor = null;
    }

    // Non-allocating enumerator pattern using struct
    public EquipmentEnumerator GetEnumerator()
    {
        return new EquipmentEnumerator(this);
    }

    public struct EquipmentEnumerator
    {
        private readonly Equipment equipment;
        private int index;

        public EquipmentEnumerator(Equipment equipment)
        {
            this.equipment = equipment;
            index = 0;
        }

        public (ItemKind kind, Item? item) Current { get; private set; }

        public bool MoveNext()
        {
            while (index < 4)
            {
                Item? item = index switch
                {
                    0 => equipment.Weapon,
                    1 => equipment.BodyArmor,
                    2 => equipment.HeadArmor,
                    3 => equipment.LegsArmor,
                    _ => default
                };

                ItemKind kind = index switch
                {
                    0 => ItemKind.Weapon,
                    1 => ItemKind.BodyArmor,
                    2 => ItemKind.HeadArmor,
                    3 => ItemKind.LegArmor,
                    _ => throw new InvalidOperationException("Invalid index")
                };

                index++;
                this.Current = (kind, item);
                return true;
            }

            return false;
        }
    }

}

public class Inventory : List<Item>
{
    public Inventory()
    {
    }

    public void AddItem(Item item)
    {
        this.Add(item);
    }

    public void RemoveItem(Item item)
    {
        this.Remove(item);
    }
}
public enum ItemKind
{
    Weapon,
    LegArmor,
    BodyArmor,
    HeadArmor,
    Consumable
}

public record struct StatTemplate
{
    public required double AttackPower { get; init; }
    public required double Defense { get; init; }
    public required int Health { get; init; }

    public StatTemplate()
    {

    }

    public static StatTemplate operator +(StatTemplate a, StatTemplate b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower + b.AttackPower,
            Defense = a.Defense + b.Defense,
            Health = a.Health + b.Health
        };
    }

    public static StatTemplate operator -(StatTemplate a, StatTemplate b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower - b.AttackPower,
            Defense = a.Defense - b.Defense,
            Health = a.Health - b.Health
        };
    }
}