namespace GameDataLayer;

public static class ItemTemplates
{
    public static Equipment HelmOfValor => new Equipment(
        "Helm of Valor",
        "A sturdy helm that provides excellent protection.",
        GoldCoin.FromAmount(100),
        new StatTemplate
        {
            AttackPower = 0,
            Defense = 5,
            Health = 0
        },
        EquipmentKind.HeadArmor);

    public static Equipment SwordOfMight => new Equipment(
        "Sword of Might",
        "A powerful sword that increases your attack power.",
        GoldCoin.FromAmount(100),
        new StatTemplate
        {
            AttackPower = 10,
            Defense = 0,
            Health = 0
        },
        EquipmentKind.Weapon);

    public static Equipment LegsOfMight => new Equipment(
        "Legs of Might",
        "A powerful legs that increases your attack power.",
        GoldCoin.FromAmount(100),
        new StatTemplate
        {
            AttackPower = 5,
            Defense = 5,
            Health = 0
        },
        EquipmentKind.LegArmor);

    public static Equipment ChestOfMight => new Equipment(
        "Chest of Might",
        "A powerful chest that increases your attack power.",
        GoldCoin.FromAmount(100),
        new StatTemplate
        {
            AttackPower = 5,
            Defense = 5,
            Health = 0
        },
        EquipmentKind.BodyArmor);
}

public class GoldCoin : IEquatable<GoldCoin>
{
    public string Name { get; } = "Gold Coin";
    public string Description { get; } = "A shiny gold coin.";
    private int amount;
    public int Amount => amount;

    private GoldCoin(int amount)
    {
        this.amount = amount;
    }

    public static GoldCoin FromAmount(int amount)
    {
        return new GoldCoin(amount);
    }

    public static GoldCoin FromRange(int min, int max)
    {
        var random = new Random();
        var amount = random.Next(min, max + 1);
        return new GoldCoin(amount);
    }

    public static GoldCoin operator +(GoldCoin a, GoldCoin b)
    {
        return new GoldCoin(a.amount + b.amount);
    }

    public static GoldCoin operator -(GoldCoin a, GoldCoin b)
    {
        return new GoldCoin(Math.Max(0, a.amount - b.amount));
    }

    public static bool operator >(GoldCoin a, GoldCoin b)
    {
        return a.amount > b.amount;
    }

    public static bool operator <(GoldCoin a, GoldCoin b)
    {
        return a.amount < b.amount;
    }

    public static bool operator >=(GoldCoin a, GoldCoin b)
    {
        return a.amount >= b.amount;
    }

    public static bool operator <=(GoldCoin a, GoldCoin b)
    {
        return a.amount <= b.amount;
    }

    public static bool operator ==(GoldCoin a, GoldCoin b)
    {
        return a.amount == b.amount;
    }

    public static bool operator !=(GoldCoin a, GoldCoin b)
    {
        return a.amount != b.amount;
    }

    public static int operator +(GoldCoin a, int b)
    {
        return a.amount + b;
    }
    public static int operator -(GoldCoin a, int b)
    {
        return Math.Max(0, a.amount - b);
    }
    public static int operator *(GoldCoin a, int b)
    {
        return a.amount * b;
    }
    public static int operator /(GoldCoin a, int b)
    {
        return a.amount / b;
    }
    public static int operator *(int a, GoldCoin b)
    {
        return a * b.amount;
    }
    public static int operator /(int a, GoldCoin b)
    {
        return a / b.amount;
    }
    public static int operator -(GoldCoin a, double b)
    {
        return (int)Math.Max(0, a.amount - b);
    }
    public static int operator *(GoldCoin a, double b)
    {
        return (int)(a.amount * b);
    }
    public static int operator /(GoldCoin a, double b)
    {
        return (int)(a.amount / b);
    }
    public static int operator *(double a, GoldCoin b)
    {
        return (int)(a * b.amount);
    }
    public static int operator /(double a, GoldCoin b)
    {
        return (int)(a / b.amount);
    }

    public bool Equals(GoldCoin? other)
    {
        if (other is null) return false;
        return this.Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is GoldCoin other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Amount.GetHashCode();
    }
}

public abstract class CraftingMaterial : IItem
{
    private static readonly Dictionary<Type, Func<int, CraftingMaterial>> registry = new();

    protected static void Register<T>(Func<int, CraftingMaterial> factory) where T : CraftingMaterial
    {
        registry[typeof(T)] = amount => factory(amount);
    }

    public string Name { get; }
    public string Description { get; }
    public GoldCoin Amount { get; set; }
    public int Count { get; set; }

    protected CraftingMaterial(string name, string description, GoldCoin amount)
    {
        Name = name;
        Description = description;
        Amount = amount;
    }

    public static CraftingMaterial FromAmount<T>(int amount) where T : CraftingMaterial
    {
        if (registry.TryGetValue(typeof(T), out var ctor))
            return ctor(amount);
        throw new ArgumentException($"Unknown material type: {typeof(T).Name}");
    }

    public static CraftingMaterial FromRange<T>(int min, int max) where T : CraftingMaterial
    {
        var random = new Random();
        var amount = random.Next(min, max + 1);
        return FromAmount<T>(amount);
    }

    public static CraftingMaterial FromAmount(Type type, int amount)
    {
        if (registry.TryGetValue(type, out var ctor))
            return ctor(amount);
        throw new ArgumentException($"Unknown material type: {type.Name}");
    }

    public static CraftingMaterial operator +(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot add different types of crafting materials.");
        return FromAmount(a.GetType(), a.Count + b.Count);
    }
    public static CraftingMaterial operator -(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot subtract different types of crafting materials.");

        if (a.Count < b.Count)
            throw new InvalidOperationException("Cannot subtract more than available.");

        return FromAmount(a.GetType(), Math.Max(0, a.Count - b.Count));
    }

    public static bool operator >(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot compare different types of crafting materials.");
        return a.Count > b.Count;
    }
    public static bool operator <(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot compare different types of crafting materials.");
        return a.Count < b.Count;
    }
    public static bool operator >=(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot compare different types of crafting materials.");
        return a.Count >= b.Count;
    }
    public static bool operator <=(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot compare different types of crafting materials.");
        return a.Count <= b.Count;
    }
    public static bool operator ==(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot compare different types of crafting materials.");
        return a.Count == b.Count;
    }
    public static bool operator !=(CraftingMaterial a, CraftingMaterial b)
    {
        if (!a.Equals(b))
            throw new InvalidOperationException("Cannot compare different types of crafting materials.");
        return a.Count != b.Count;
    }

    public static int operator +(CraftingMaterial a, int b)
    {
        return a.Count + b;
    }

    public static int operator -(CraftingMaterial a, int b)
    {
        return Math.Max(0, a.Count - b);
    }

    public static int operator *(CraftingMaterial a, int b)
    {
        return a.Count * b;
    }

    public static int operator /(CraftingMaterial a, int b)
    {
        return a.Count / b;
    }
    

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is CraftingMaterial other)
        {
            return this.Name == other.Name;
        }
        return false;
    }
}

public abstract class Consumable : IItem
{
    protected string name;
    protected string description;

    public string Name => name;
    public string Description => description;

    public GoldCoin Amount { get; set; }

    public Consumable(string name, string description, GoldCoin amount)
    {
        this.name = name;
        this.description = description;
        this.Amount = amount;
    }

    public abstract void Use(CharacterBase character);
}

public abstract class HealthPotionBase : Consumable
{
    public int HealthRestored { get; }

    public HealthPotionBase(int healthRestored, string name, string description, GoldCoin amount) : base(name, description, amount)
    {
        HealthRestored = healthRestored;
    }

    public override void Use(CharacterBase character)
    {
        character.CurrentHealth += 50;
    }

    public class MinorHealthPotion : HealthPotionBase
    {
        public MinorHealthPotion() : base(50, "Minor Health Potion", "Restores a small amount of health.", GoldCoin.FromAmount(50))
        {
        }
    }

    public class SuperHealthPotion : HealthPotionBase
    {
        public SuperHealthPotion() : base(100, "Super Health Potion", "Restores a massive amount of health.", GoldCoin.FromAmount(500))
        {
        }
    }

    public class MajorHealthPotion : HealthPotionBase
    {
        public MajorHealthPotion() : base(200, "Major Health Potion", "Restores a large amount of health.", GoldCoin.FromAmount(2000))
        {
        }
    }
}