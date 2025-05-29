#nullable enable
using static GameData.Inventory.EquipmentManager;

namespace GameData.Inventory;

public class EquipmentManager : IStateSerializable<EquipmentManagerDto, EquipmentManager>
{
    private readonly Dictionary<EquipmentKind, Equipment?> _slots = new()
    {
        { EquipmentKind.Weapon, null },
        { EquipmentKind.BodyArmor, null },
        { EquipmentKind.HeadArmor, null },
        { EquipmentKind.LegArmor, null }
    };

    public Equipment? this[EquipmentKind kind]
    {
        get => _slots.TryGetValue(kind, out var item) ? item : null;
        set
        {
            if (!_slots.ContainsKey(kind))
                throw new ArgumentOutOfRangeException(nameof(kind), "Invalid equipment kind.");
            _slots[kind] = value;
        }
    }

    public Equipment? Equip(Equipment item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        var kind = item.Kind;
        if (!_slots.ContainsKey(kind))
            throw new InvalidOperationException("Unsupported equipment kind.");

        var previous = _slots[kind];
        _slots[kind] = item;
        return previous;
    }

    public Equipment? Unequip(EquipmentKind kind)
    {
        if (!_slots.ContainsKey(kind))
            throw new InvalidOperationException("Invalid equipment kind.");

        var current = _slots[kind];
        _slots[kind] = null;
        return current;
    }

    public IEnumerable<(EquipmentKind Kind, Equipment? Item)> GetAllEquipment() => _slots.Select(kvp => (kvp.Key, kvp.Value));

    public static StatTemplate operator +(EquipmentManager equipment, StatTemplate baseStats)
    {
        return equipment.GetAllEquipment()
                        .Where(e => e.Item is not null)
                        .Aggregate(baseStats, (current, e) => current + e.Item!.Stats);
    }

    public static StatTemplate operator +(StatTemplate baseStats, EquipmentManager equipment) =>
        equipment + baseStats;

    public readonly struct EquipmentManagerDto
    {
        public Dictionary<EquipmentKind, EquipmentDto?> Slots { get; init; }
    }
    public EquipmentManagerDto Serialize()
    {
        return new EquipmentManagerDto
        {
            Slots = _slots.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Serialize())
        };
    }

    public static EquipmentManager Restore(EquipmentManagerDto dto)
    {
        var manager = new EquipmentManager();
        foreach (var kvp in dto.Slots)
        {
            if (kvp.Value is EquipmentDto equipmentDto)
            {
                manager._slots[kvp.Key] = Equipment.Restore(equipmentDto);
            }
            else
            {
                manager._slots[kvp.Key] = null;
            }
        }
        return manager;
    }
}
