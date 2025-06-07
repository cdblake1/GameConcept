public abstract class ActorBase : IActor
{
    public string Name { get; }
    public int MaxHealth => Stats.Health;
    public abstract int CurrentHealth { get; set; }
    public readonly StatTemplateOld baseStats;
    public readonly List<Effect> Effects = new();
    public abstract StatTemplateOld Stats { get; }
    public int Level => level;
    private readonly int level;

    public StatTemplateOld BaseStats => this.baseStats;

    protected ActorBase(IActor actor, int level)
    {
        this.level = level;
        this.baseStats = new()
        {
            AttackPower = actor.BaseStats.AttackPower,
            Defense = actor.BaseStats.Defense,
            Health = actor.BaseStats.Health,
            Speed = actor.BaseStats.Speed
        };

        this.Name = actor.Name;
    }

    public virtual void ApplyEffect(Effect[] effects)
    {
        foreach (var effect in effects)
        {
            if (effect is StatusEffect statusEffect)
            {
                this.Effects.Add(statusEffect);
            }

            else if (effect is DamageEffect damageEffect)
            {
                var damage = damageEffect.Damage();

                CurrentHealth -= (int)Math.Max(0, damage - Stats.Defense);
            }
        }
    }

    public virtual int ApplyDamage(DamageEffect damageEffect)
    {
        var damage = damageEffect.Damage();
        CurrentHealth = Math.Max(0, CurrentHealth - Math.Max(0, (int)(damage - Stats.Defense)));
        return (int)Math.Max(0, damage - Stats.Defense);
    }
}