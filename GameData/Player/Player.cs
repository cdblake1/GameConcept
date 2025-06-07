#nullable enable

using GameData.Inventory;
using static GameData.Inventory.EquipmentManager;
using static GameData.LevelManager;
using static GameData.PlayerOld;
using static InventoryManager;

namespace GameData;

public class PlayerOld : CharacterBase, IStateSerializable<PlayerDto, PlayerOld>
{
    private static string actorId => "Player";
    public Guid Id { get; private set; }

    private static StatTemplateOld stats => new()
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
    public PlayerOld(string name) : base(name, actorId, stats, levelManager)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }

        Id = Guid.NewGuid();
        Class = null;
        CurrentHealth = MaxHealth;
    }

    private PlayerOld(PlayerDto dto) : base(dto.Name, dto.ActorId, stats, levelManager)
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

    public static PlayerOld Restore(PlayerDto dto)
    {
        return new PlayerOld(dto); ;
    }
}