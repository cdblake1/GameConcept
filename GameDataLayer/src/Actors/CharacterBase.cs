using GameDataLayer;

public abstract class CharacterBase : ActorBase
{
    public EquipmentManager Equipment { get; } = new EquipmentManager();
    public InventoryManager Inventory { get; } = new InventoryManager();
    public LevelManager Level { get; set; }
    public GoldCoin Gold { get; set; } = GoldCoin.FromAmount(0);
    public override StatTemplate Stats => StatScaler.Scale(backingStats, Level.CurrentLevel, 15, GrowthModel.Smoothed, 1, 0.05, false);

    private StatTemplate backingStats => this.baseStats + this.Equipment;
    protected CharacterBase(string name, string actorId, StatTemplate baseStats, LevelManager levelManager) : base(name, baseStats, actorId)
    {
        Level = levelManager ?? throw new ArgumentNullException(nameof(levelManager));

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