#nullable enable
using GameData;
using GameData.Inventory;

public abstract class CharacterBase : ActorBase
{
    public EquipmentManager Equipment { get; init; } = new EquipmentManager();

    public InventoryManager Inventory { get; init; } = new InventoryManager();

    public LevelManager LevelManager { get; set; }
    public Class? Class { get; set; }

    public GoldCoin Gold { get; set; } = GoldCoin.FromAmount(0);

    public override StatTemplate Stats => this.backingStats + this.Equipment;

    private StatTemplate backingStats => this.baseStats + this.Equipment;

    protected CharacterBase(string name, string actorId, StatTemplate baseStats, LevelManager levelManager) : base(new MobDto(name, baseStats), 1)
    {
        this.LevelManager = levelManager ?? throw new ArgumentNullException(nameof(levelManager));
    }

    public void EquipItem(Equipment item)
    {
        if (!Inventory.Equipment.Contains(item))
        {
            throw new InvalidOperationException("Item must be in inventory to equip.");
        }

        Inventory.RemoveItem(item);
        Equipment.Equip(item);
    }

    public void UnequipItem(EquipmentKind kind)
    {
        var item = Equipment.Unequip(kind);
        if (item != null)
        {
            Inventory.AddItem(item);
        }
    }
}