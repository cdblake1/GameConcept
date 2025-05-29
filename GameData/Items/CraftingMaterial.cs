#nullable enable

using GameData;
using static GameData.CraftingMaterial;

namespace GameData;

public abstract class CraftingMaterial : IItem, IStateSerializable<CraftingMaterialDto, CraftingMaterial>
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

    public ItemRarity Rarity { get; set; }

    protected CraftingMaterial(string name, string description, GoldCoin amount, ItemRarity rarity)
    {
        Name = name;
        Description = description;
        Amount = amount;
        Rarity = rarity;
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

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public struct CraftingMaterialDto
    {
        public string Type { get; set; }
        public int Count { get; set; }
    }

    public CraftingMaterialDto Serialize()
    {
        return new CraftingMaterialDto
        {
            Type = this.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException("Type is null"),
            Count = this.Count
        };
    }

    public static CraftingMaterial Restore(CraftingMaterialDto dto)
    {
        return FromAmount(Type.GetType(dto.Type) ?? throw new ArgumentException($"Unknown material type: {dto.Type}"), dto.Count);
    }
}