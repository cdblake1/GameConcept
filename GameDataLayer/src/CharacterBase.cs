using System.Dynamic;
using GameDataLayer;

namespace GameDataLayer;

public class CharacterBase
{
    private readonly string name;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; set; }
    public string Name => name;

    public GoldCoin gold = GoldCoin.FromAmount(0);
    public GoldCoin Gold => gold;

    public void AddGold(GoldCoin amount)
    {
        gold += amount;
    }

    public void RemoveGold(GoldCoin amount)
    {
        gold -= amount;
    }

    private readonly StatTemplate baseStats;
    public StatTemplate Stats => baseStats + EquipmentManager + LevelManager.Stats;

    private readonly Inventory inventory = new();

    public Inventory Inventory => inventory;

    public readonly EquipmentManager EquipmentManager = new();

    public LevelManager LevelManager { get; }

    public CharacterBase(string name, StatTemplate baseStats, LevelManager level)
    {
        this.baseStats = baseStats;
        MaxHealth = baseStats.Health;
        CurrentHealth = MaxHealth;
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");
        }

        this.name = name;
        LevelManager = level;
    }

    public void EquipItem(Equipment item)
    {
        if (!inventory.Contains(item))
        {
            throw new InvalidOperationException("Item must be in inventory to equip.");
        }

        inventory.Remove(item);
        Equipment? unequippedItem;

        switch (item.Kind)
        {
            case EquipmentKind.Weapon:
                unequippedItem = EquipmentManager.Weapon;
                EquipmentManager.Weapon = item;
                break;
            case EquipmentKind.BodyArmor:
                unequippedItem = EquipmentManager.BodyArmor;
                EquipmentManager.BodyArmor = item;
                break;
            case EquipmentKind.HeadArmor:
                unequippedItem = EquipmentManager.HeadArmor;
                EquipmentManager.HeadArmor = item;
                break;
            case EquipmentKind.LegArmor:
                unequippedItem = EquipmentManager.LegsArmor;
                EquipmentManager.LegsArmor = item;
                break;
            default:
                throw new InvalidOperationException("Item cannot be equipped.");
        }

        if (unequippedItem is Equipment eq)
        {
            inventory.Add(eq);
        }
    }

    public void UnequipItem(EquipmentKind kind)
    {
        Equipment? item = null;
        switch (kind)
        {
            case EquipmentKind.Weapon:
                this.EquipmentManager.Weapon = null;
                item = EquipmentManager.Weapon;
                break;
            case EquipmentKind.BodyArmor:
                this.EquipmentManager.BodyArmor = null;
                item = EquipmentManager.BodyArmor;
                break;
            case EquipmentKind.HeadArmor:
                this.EquipmentManager.HeadArmor = null;
                item = EquipmentManager.HeadArmor;
                break;
            case EquipmentKind.LegArmor:
                this.EquipmentManager.LegsArmor = null;
                item = EquipmentManager.LegsArmor;
                break;
            default:
                throw new InvalidOperationException("Item cannot be unequipped.");
        }

        if (item is not Equipment equippedItem)
        {
            throw new InvalidOperationException("Item is not equipped.");
        }
        else
        {
            inventory.Add(equippedItem);
        }
    }

    public double Attack(CharacterBase target)
    {
        double damage = Math.Max(0, this.Stats.AttackPower - target.Stats.Defense);
        target.CurrentHealth = Math.Max(0, target.CurrentHealth - (int)damage);
        return damage;
    }
}

public interface IItem
{
    string Name { get; }
    string Description { get; }
    GoldCoin Amount { get; }
}

public interface CraftingRecipe
{
    public ICraftedItem CraftedItem { get; init; }
}

public interface ICraftedItem : IItem
{
    public List<IItem> RequiredMaterials { get; init; }
    public int CraftingTime { get; init; }
}

public static class CraftedEquipmentTemplates
{
    public class IronHelmet : Equipment
    {

    }
}

public static class CraftingMaterialTemplates
{
    public class IronScrap : IItem
    {
        public string Name => "Iron Scrap";
        public string Description => "A piece of iron scrap.";
        public GoldCoin Amount { get; init; } = GoldCoin.FromAmount(10);

        public IronScrap()
        {
        }
    }

    public class WoodenShoot : IItem
    {
        public string Name => "Wooden Shoot";
        public string Description => "A piece of wooden shoot.";
        public GoldCoin Amount { get; init; } = GoldCoin.FromAmount(10);

        public WoodenShoot()
        {
        }
    }

    public class TatteredCloth : IItem
    {
        public string Name => "Tattered Cloth";
        public string Description => "A piece of tattered cloth.";
        public GoldCoin Amount { get; init; } = GoldCoin.FromAmount(10);

        public TatteredCloth()
        {
        }
    }
}

public class LevelManager
{
    private readonly int maxLevel;
    private readonly ExperienceTable experienceTable;

    public int CurrentLevel { get; private set; }
    public int NextLevel => CurrentLevel + 1;
    public int CurrentExperience { get; private set; }

    public int MaxLevel => maxLevel;

    public StatTemplate StatsPerLevel { get; private set; }

    public StatTemplate Stats => StatsPerLevel * (CurrentLevel - 1);

    public LevelManager(int maxLevel, ExperienceTable experienceTable, StatTemplate levelStatGrowth, int startingLevel = 1)
    {
        if (maxLevel <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLevel), "Max level must be greater than 0.");
        }

        this.maxLevel = maxLevel;
        this.experienceTable = experienceTable;
        this.StatsPerLevel = levelStatGrowth;
        this.CurrentLevel = startingLevel;
        CurrentExperience = 0;
    }

    public bool AddExperience(int experience)
    {
        if (CurrentLevel >= maxLevel)
        {
            return false;
        }

        CurrentExperience = Math.Max(CurrentExperience, CurrentExperience + experience);
        int newLevel = experienceTable.GetLevelByExperience(CurrentExperience);

        if (newLevel > CurrentLevel)
        {
            CurrentLevel = Math.Min(newLevel, maxLevel);
            if (CurrentLevel == maxLevel)
            {
                CurrentExperience = experienceTable.GetCumulativeExperienceForLevel(maxLevel);
            }
            return true;
        }

        return false;
    }

    public int GetExperienceNeededForNextLevel()
    {
        return CurrentLevel >= maxLevel
            ? 0
            : experienceTable.GetCumulativeExperienceForLevel(CurrentLevel + 1);
    }
}

public class MobBase : CharacterBase
{
    private readonly LootTable lootTable;

    private readonly int baseExperience;

    public MobBase(string name, StatTemplate baseStats, LootTable lootTable, LevelManager level, int baseExperience) : base(name, baseStats, level)
    {
        this.lootTable = lootTable ?? throw new ArgumentNullException(nameof(lootTable), "Loot table cannot be null.");
        this.baseExperience = baseExperience;
    }

    public Equipment? DropLoot()
    {
        var lootEntry = lootTable.GetRandomLootEntry();
        if (lootEntry is null)
        {
            return null;
        }

        var item = lootEntry.Value.Item;
        Inventory.AddItem(item);
        return item;
    }

    public int AwardExperience()
    {
        return baseExperience + (int)(baseExperience * (this.LevelManager.CurrentLevel - 1) * 0.1);
    }
}

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

public class EquipmentManager
{
    public Equipment? Weapon { get; set; }
    public Equipment? BodyArmor { get; set; }
    public Equipment? HeadArmor { get; set; }
    public Equipment? LegsArmor { get; set; }

    public EquipmentManager()
    {
        Weapon = null;
        BodyArmor = null;
        HeadArmor = null;
        LegsArmor = null;
    }

    public Equipment? this[EquipmentKind kind]
    {
        get => kind switch
        {
            EquipmentKind.Weapon => Weapon,
            EquipmentKind.BodyArmor => BodyArmor,
            EquipmentKind.HeadArmor => HeadArmor,
            EquipmentKind.LegArmor => LegsArmor,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), "Invalid item kind.")
        };
        set
        {
            switch (kind)
            {
                case EquipmentKind.Weapon:
                    Weapon = value;
                    break;
                case EquipmentKind.BodyArmor:
                    BodyArmor = value;
                    break;
                case EquipmentKind.HeadArmor:
                    HeadArmor = value;
                    break;
                case EquipmentKind.LegArmor:
                    LegsArmor = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), "Invalid item kind.");
            }
        }
    }

    public IEnumerable<(EquipmentKind kind, Equipment? item)> GetAllEquipment()
    {
        yield return (EquipmentKind.Weapon, Weapon);
        yield return (EquipmentKind.BodyArmor, BodyArmor);
        yield return (EquipmentKind.HeadArmor, HeadArmor);
        yield return (EquipmentKind.LegArmor, LegsArmor);
    }

    public static StatTemplate operator +(EquipmentManager equipment, StatTemplate baseStats)
    {
        return equipment.GetAllEquipment()
                        .Where(e => e.item is not null)
                        .Aggregate(baseStats, (current, e) => current + e.item!.Stats);
    }

    public static StatTemplate operator +(StatTemplate baseStats, EquipmentManager equipment)
    {
        return equipment + baseStats;
    }
}

public class Inventory : List<IItem>
{
    public Inventory()
    {
    }

    public void AddItem(IItem item)
    {
        this.Add(item);
    }

    public void RemoveItem(IItem itemToRemove)
    {
        if (this.Contains(itemToRemove))
        {
            this.Remove(itemToRemove);
        }
        else
        {
            throw new InvalidOperationException("Item not found in inventory.");
        }
    }

    public IReadOnlyList<Equipment> GetItemsMatchingKind(EquipmentKind kind)
    {
        return this.OfType<Equipment>().Where(item => item.Kind == kind).ToList();
    }
}

public enum EquipmentKind
{
    Weapon,
    LegArmor,
    BodyArmor,
    HeadArmor,
}

public class ExperienceTable
{
    public static ExperienceTable Default => new ExperienceTable(new Dictionary<int, int>
    {
        { 1, 0 },
        { 2, 100 },
        { 3, 300 },
        { 4, 600 },
        { 5, 1000 },
        { 6, 1500 },
        { 7, 2100 },
        { 8, 2800 },
        { 9, 3600 },
        { 10, 4500 },
        { 11, 5500 },
        { 12, 6600 },
        { 13, 7800 },
        { 14, 9100 },
        { 15, 10500 },
        { 16, int.MaxValue }
    });

    public Dictionary<int, int> Table { get; }

    private readonly int maxLevel;
    private readonly int minLevel;

    public ExperienceTable(Dictionary<int, int> experienceTable)
    {
        Table = experienceTable ?? throw new ArgumentNullException(nameof(experienceTable), "Experience table cannot be null.");
        maxLevel = experienceTable.Keys.Max();
        minLevel = experienceTable.Keys.Min();
    }

    public int GetCumulativeExperienceForLevel(int level)
    {
        if (level < minLevel || level > maxLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be within the valid range.");
        }

        return Table.Where(entry => entry.Key <= level).Sum(entry => entry.Value);
    }

    public int GetLevelByExperience(int experience)
    {
        foreach (var entry in Table.OrderByDescending(e => e.Key))
        {
            if (experience >= entry.Value)
            {
                return entry.Key;
            }
        }

        return minLevel;
    }
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

    public static StatTemplate operator *(StatTemplate a, int b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower * b,
            Defense = a.Defense * b,
            Health = a.Health * b
        };
    }
}