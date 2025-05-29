#nullable enable

using GameData.Inventory;
using static GameData.Inventory.EquipmentManager;
using static GameData.LevelManager;
using static GameData.Player;
using static InventoryManager;

namespace GameData;

public class Player : CharacterBase, IStateSerializable<PlayerDto, Player>
{
    private static string actorId => "Player";
    public Guid Id { get; private set; }

    private static StatTemplate stats => new()
    {
        Health = 200,
        AttackPower = 30,
        Defense = 0,
        Speed = 2,
    };

    private static LevelManager levelManager => new(
        maxLevel: 15,
        experienceTable: ExperienceTable.PlayerExpTable,
        startingLevel: 1);

    public override int CurrentHealth { get; set; }

    // Constructor for creating a new Player
    public Player(string name) : base(name, actorId, stats, levelManager)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }

        Id = Guid.NewGuid();
        Class = null;
        CurrentHealth = MaxHealth;
    }

    private Player(PlayerDto dto) : base(dto.Name, dto.ActorId, stats, levelManager)
    {
        Id = Guid.NewGuid();
        Class = dto.Class != null ? Class.Restore(dto.Class.Value) : null;
        CurrentHealth = dto.CurrentHealth;
        LevelManager = LevelManager.Restore(dto.Level);
        Equipment = EquipmentManager.Restore(dto.Equipment);
        Inventory = InventoryManager.Restore(dto.Inventory);
    }

    public readonly struct PlayerDto
    {
        public string Name { get; init; }
        public string ActorId { get; init; }
        public int MaxHealth { get; init; }
        public int CurrentHealth { get; init; }
        public Class.ClassDto? Class { get; init; }
        public LevelManagerDto Level { get; init; }
        public EquipmentManagerDto Equipment { get; init; }
        public InventoryManagerDto Inventory { get; init; }
    }

    public PlayerDto Serialize()
    {
        return new PlayerDto
        {
            Name = Name,
            MaxHealth = MaxHealth,
            CurrentHealth = CurrentHealth,
            Class = Class?.Serialize(),
            Level = LevelManager.Serialize(),
            Equipment = Equipment.Serialize(),
            Inventory = Inventory.Serialize(),
        };
    }

    public static Player Restore(PlayerDto dto)
    {
        var player = new Player(dto);

        var equipment = EquipmentManager.Restore(dto.Equipment);
        foreach (var (kind, item) in equipment.GetAllEquipment())
        {
            if (item is null)
            {
                continue;
            }

            player.Equipment.Equip(item);
        }

        var inventory = InventoryManager.Restore(dto.Inventory);
        foreach (var item in inventory.Equipment)
        {
            player.Inventory.AddItem(item);
        }

        foreach (var item in inventory.CraftingMaterials)
        {
            player.Inventory.AddItem(item);
        }

        return player;
    }
}