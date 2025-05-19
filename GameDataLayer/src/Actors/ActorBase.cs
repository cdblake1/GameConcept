public abstract class ActorBase
{
    public string Name { get; }
    public int MaxHealth => Stats.Health;
    public abstract int CurrentHealth { get; set; }
    public string ActorId { get; }

    protected readonly StatTemplate baseStats;

    public abstract StatTemplate Stats { get; }

    protected ActorBase(string name, StatTemplate baseStats, string actorId)
    {
        ActorId = actorId ?? throw new ArgumentNullException(nameof(actorId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        this.baseStats = baseStats;
    }

    public virtual double Attack(ActorBase target)
    {
        double damage = Math.Max(0, Stats.AttackPower - target.Stats.Defense);
        target.CurrentHealth -= (int)damage;
        return damage;
    }
}