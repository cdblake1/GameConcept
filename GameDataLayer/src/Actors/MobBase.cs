namespace GameDataLayer;

public abstract class MobBase : ActorBase
{
    protected LootTable LootTable { get; init; }
    private readonly int baseExperience;

    public int Level { get; init; }

    public override int CurrentHealth { get; set; }

    public override StatTemplate Stats => StatScaler.Scale(baseStats, Level, 15, GrowthModel.Smoothed, 1, 0.05, false);

    protected MobBase(string name, string actorId, int baseExperience, StatTemplate baseStats, LootTable LootTable) : base(name, baseStats, actorId)
    {
        this.LootTable = LootTable ?? throw new ArgumentNullException(nameof(LootTable));
        this.baseExperience = baseExperience;
        this.CurrentHealth = MaxHealth;
    }

    public virtual IItem? DropLoot()
    {
        var lootEntry = LootTable.GetRandomLootEntry();
        if (lootEntry is null)
        {
            return null;
        }

        return lootEntry.Value.Item;
    }

    public virtual int AwardExperience()
    {
        return baseExperience + (int)(baseExperience * this.Level * 0.1);
    }
}