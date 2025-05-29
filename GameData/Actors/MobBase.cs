namespace GameData;

public abstract class MobBase : ActorBase
{
    protected LootTable LootTable { get; init; }
    public IReadOnlyList<Skill> AttackSkill { get; }

    public override int CurrentHealth { get; set; }

    public override StatTemplate Stats => this.baseStats;

    protected MobBase(IActor actor, LootTable LootTable, IReadOnlyList<Skill> skills, int level) : base(actor, level)
    {
        this.LootTable = LootTable ?? throw new ArgumentNullException(nameof(LootTable));
        this.CurrentHealth = MaxHealth;
        this.AttackSkill = skills ?? throw new ArgumentNullException(nameof(skills));
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

    public virtual int AwardExperience() => ExperienceTable.MonsterExpTable[Level];
}