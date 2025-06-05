public interface IClass
{
    public abstract string Name { get; }
    public abstract string Description { get; }

    public abstract List<Talent> Talents { get; }
    public abstract List<(int requiredLevel, Skill Skill)> SkillList { get; }
}

public abstract class Class : IClass, IStateSerializable<Class.ClassDto, Class>
{
    public abstract string Name { get; }
    public abstract string Description { get; }

    public abstract List<Talent> Talents { get; }
    public abstract List<(int requiredLevel, Skill Skill)> SkillList { get; }

    public readonly struct ClassDto
    {
        public string Name { get; init; }
    }

    public ClassDto Serialize()
    {
        return new ClassDto
        {
            Name = this.Name,
        };
    }

    public static Class Restore(ClassDto dto)
    {
        return dto.Name switch
        {
            "BloodReaver" => new BloodReaver(),
            _ => throw new NotImplementedException($"Class {dto.Name} not implemented."),
        };
    }
}


public abstract class Talent
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract bool ShouldApply(Skill skill);
    public abstract void Apply(SkillPropertySnapshot snapshot, Skill skill);
    protected readonly ActorBase Originator;

    public Talent(ActorBase originator)
    {
        this.Originator = originator ?? throw new ArgumentNullException(nameof(originator), "Originator cannot be null.");
    }
}

public abstract class Skill
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public readonly IReadOnlyList<Effect> Effects = [];

    private readonly List<Effect> effects = [];

    public Skill()
    {
    }

    public abstract IReadOnlyList<Effect> Apply(SkillPropertySnapshot snapshot);

    public virtual void AddEffect(Effect effect)
    {
        if (effect == null) throw new ArgumentNullException(nameof(effect), "Effect cannot be null.");
        effects.Add(effect);
    }

    public virtual void RemoveEffect(Effect effect)
    {
        if (effect == null) throw new ArgumentNullException(nameof(effect), "Effect cannot be null.");
        effects.Remove(effect);
    }
}

public abstract class Effect
{
    public abstract string Name { get; }
    public abstract string Description { get; }
}

public abstract class SkillModifierOld : Effect
{
    public override string Name { get; } = "Skill Modifier";
    public override string Description { get; } = "Modifies the skill's base damage.";
    public abstract SkillPropertySnapshot Apply();
}

public abstract class StatusEffect : Effect
{
    public abstract int Duration { get; }

    protected SkillPropertySnapshot propertySnapshot;

    public virtual void ProvideSnapshot(SkillPropertySnapshot snapshot)
    {
        this.propertySnapshot = snapshot;
    }

    public StatusEffect(SkillPropertySnapshot snapshot)
    {

        this.propertySnapshot = snapshot;
    }

    public abstract double Tick();
    public abstract double Expire();
}

public abstract class DamageEffect : Effect
{
    public override string Name { get; } = "Damage Effect";
    public override string Description { get; } = "Applies damage to the target.";
    public abstract double Damage();

    public static DamageEffect From(SkillPropertySnapshot snapshot)
    {
        return new BaseDamageEffect(snapshot);
    }

    protected SkillPropertySnapshot propertySnapshot;

    public DamageEffect(SkillPropertySnapshot snapshot)
    {
        this.propertySnapshot = snapshot;
    }

    private class BaseDamageEffect : DamageEffect
    {

        public BaseDamageEffect(SkillPropertySnapshot snapshot) : base(snapshot)
        {
        }

        public override double Damage()
        {
            return this.propertySnapshot.AddedBaseDamage *
                (1 + this.propertySnapshot.AddedBaseDamageMultiplier) *
                this.propertySnapshot.Multiplier;
        }
    }
}

public class BloodReaver : Class
{
    public const string Identifier = "BloodReaver";
    public override string Name => "BloodReaver";
    public override string Description { get; } = "A master of blood magic, the Blood Reaver can drain the life force from enemies and use it to empower themselves.";
    public override List<Talent> Talents { get; } = [];
    public override List<(int requiredLevel, Skill Skill)> SkillList { get; }

    public BloodReaver()
    {
        SkillList = new()
        {
            (1, new Slash()),
            (2, new Rend()),
        };
    }
}

public class Rend : Skill
{
    public override string Name { get; } = "Rend";
    public override string Description =>
        $"A powerful slash that rends the target, dealing {BaseDamageCoefficient.MinScalingFactor} of base damage to the target and " +
        $"{BleedDamageCoefficient.MinScalingFactor} to {BleedDamageCoefficient.MaxScalingFactor} of base damage as bleeding damage over time. " +
        $"Lasts for {duration} rounds.";

    private int duration = 3;

    private SkillCoefficient BaseDamageCoefficient { get; } = new SkillCoefficient(0.60);
    private SkillCoefficient BleedDamageCoefficient { get; } = new SkillCoefficient(1.40, 1.50);

    public Rend() : base()
    {
    }

    public override IReadOnlyList<Effect> Apply(SkillPropertySnapshot snapshot)
    {
        var effectsToReturn = new List<Effect>();

        foreach (var effect in Effects)
        {
            if (effect is SkillModifierOld modifier)
            {
                snapshot += modifier.Apply();
            }
        }

        foreach (var effect in Effects)
        {
            if (effect is StatusEffect statusEffect)
            {
                statusEffect.ProvideSnapshot(snapshot);
                effectsToReturn.Add(statusEffect);
            }
        }

        var baseDamage = snapshot.AddedBaseDamage * (1 + snapshot.AddedBaseDamageMultiplier);
        var initialDamage = (int)(baseDamage * (Random.Shared.NextDouble() *
            (BaseDamageCoefficient.MaxScalingFactor - BaseDamageCoefficient.MinScalingFactor) + BaseDamageCoefficient.MinScalingFactor));
        var bleedDamage = (int)(baseDamage * (Random.Shared.NextDouble() *
            (BleedDamageCoefficient.MaxScalingFactor - BleedDamageCoefficient.MinScalingFactor) + BleedDamageCoefficient.MinScalingFactor));

        effectsToReturn.Add(new RendDamageEffect(snapshot));
        effectsToReturn.Add(new RendBleedEffect(snapshot, duration));

        return effectsToReturn;
    }

    public class RendDamageEffect : DamageEffect
    {
        public override string Name { get; } = "Rend Damage";
        public override string Description { get; } = "Deals initial damage and applies bleeding damage over time.";

        private readonly SkillCoefficient BaseDamageCoefficient = new SkillCoefficient(0.60);
        public RendDamageEffect(SkillPropertySnapshot snapshot) : base(snapshot)
        {
        }

        public override double Damage()
        {
            return this.propertySnapshot.AddedBaseDamage *
                (1 + this.propertySnapshot.AddedBaseDamageMultiplier) *
                (Random.Shared.NextDouble() * (BaseDamageCoefficient.MaxScalingFactor - BaseDamageCoefficient.MinScalingFactor) + BaseDamageCoefficient.MinScalingFactor);
        }
    }

    public class RendBleedEffect : StatusEffect
    {
        private int remainingDuration;

        public override int Duration { get; }

        public override string Name => "Rend";

        public override string Description => "A bleed over time effect lasting some duration";

        public RendBleedEffect(SkillPropertySnapshot snapshot, int duration) : base(snapshot)
        {
            remainingDuration = duration;
            Duration = duration;
        }

        public override double Tick()
        {
            if (remainingDuration > 0)
            {
                remainingDuration--;
                return propertySnapshot.AddedBaseDamage / Duration;
            }

            return 0;
        }

        public override double Expire()
        {
            return 0;
        }
    }
}

public class Slash : Skill
{
    public override string Name { get; } = "Slash";
    public override string Description => $"A quick slash with a weapon that deals {coefficient.MinScalingFactor} to {coefficient.MaxScalingFactor} of base damage to the target.";

    public SkillCoefficient coefficient { get; } = new SkillCoefficient(1.05, 1.10);

    public Slash() : base()
    {
    }

    public override IReadOnlyList<Effect> Apply(SkillPropertySnapshot snapshot)
    {
        var effectsToReturn = new List<Effect>();

        foreach (var effect in Effects)
        {
            if (effect is SkillModifierOld modifier)
            {
                snapshot += modifier.Apply();
            }
        }

        foreach (var effect in Effects)
        {
            if (effect is StatusEffect statusEffect)
            {
                statusEffect.ProvideSnapshot(snapshot);
            }

            effectsToReturn.Add(effect);
        }

        effectsToReturn.Add(new SlashDamageEffect(snapshot, coefficient));

        return effectsToReturn;
    }

    public class SlashDamageEffect : DamageEffect
    {
        public override string Name { get; } = "Slash Damage";
        public override string Description { get; } = "Deals damage to the target based on the scaling factor of the Slash skill.";

        private readonly SkillCoefficient skillCoefficient;

        public SlashDamageEffect(SkillPropertySnapshot snapshot, SkillCoefficient skillCoefficient) : base(snapshot)
        {
            this.propertySnapshot = snapshot;
            this.skillCoefficient = skillCoefficient;
        }

        public override double Damage()
        {
            return (int)(propertySnapshot.AddedBaseDamage * this.CalculateCoefficient());
        }

        private double CalculateCoefficient()
        {
            return Random.Shared.NextDouble() * (skillCoefficient.MaxScalingFactor - skillCoefficient.MinScalingFactor) + skillCoefficient.MinScalingFactor;
        }
    }
}

public record SkillCoefficient
{
    public double MinScalingFactor { get; set; }
    public double MaxScalingFactor { get; set; }

    public SkillCoefficient(double minScalingFactor, double? maxScalingFactor = null)
    {
        MinScalingFactor = minScalingFactor;
        MaxScalingFactor = maxScalingFactor ?? minScalingFactor;

        if (MinScalingFactor > MaxScalingFactor)
        {
            throw new ArgumentException("MinScalingFactor cannot be greater than MaxScalingFactor.");
        }
    }
}

public class SlashTalentOne : Talent
{
    public override string Name { get; } = "Slash Talent One";
    public override string Description { get; } = "Increases the damage of the Slash skill by 10%.";

    public override void Apply(SkillPropertySnapshot snapshot, Skill skill)
    {
        if (skill is not Slash slash)
        {
            skill.AddEffect(new SlashOneMultiplierEffect());
            return;
        }
        else
        {
            throw new InvalidOperationException("Talent can only be applied to Slash skill.");
        }
    }

    public override bool ShouldApply(Skill skill)
    {
        throw new NotImplementedException();
    }

    public SlashTalentOne(ActorBase originator) : base(originator)
    {
    }

    public class SlashOneMultiplierEffect : SkillModifierOld
    {
        public override string Name { get; } = "Slash One Multiplier Effect";
        public override string Description { get; } = "Increases the damage of the Slash skill by 10%.";

        public SlashOneMultiplierEffect()
        {
        }

        public override SkillPropertySnapshot Apply()
        {
            return new()
            {
                Multiplier = 0.10,
            };
        }
    }
}

public class SlashTalentTwo : Talent
{
    public override string Name { get; } = "Slash Talent Two";
    public override string Description => "reduces base damage of Slash by 20% but slash now applies a bleed effect that deals 50% of base damage over 2 rounds.";

    public override void Apply(SkillPropertySnapshot snapshot, Skill skill)
    {
        if (skill is not Slash slash)
        {
            throw new InvalidOperationException("Talent can only be applied to Slash skill.");
        }

        slash.AddEffect(new SlashBleedEffect(snapshot));
    }

    public override bool ShouldApply(Skill skill)
    {
        if (skill is Slash)
        {
            return true;
        }
        return false;
    }

    public SlashTalentTwo(ActorBase originator) : base(originator)
    {
    }

    public class SlashBaseDamageEffect : SkillModifierOld
    {
        public override SkillPropertySnapshot Apply()
        {
            return new()
            {
                AddedBaseDamageMultiplier = -0.20,
            };
        }
    }

    public class SlashBleedEffect : StatusEffect
    {
        public override int Duration { get; } = 2;

        public override string Name => "Slash";

        public override string Description => "A bleed over time effect applied by Slash";

        private int remainingDuration;

        public SlashBleedEffect(SkillPropertySnapshot propertySnapshot) : base(propertySnapshot)
        {
            remainingDuration = Duration;
        }

        public override double Tick()
        {
            if (remainingDuration > 0)
            {
                remainingDuration--;
                return (int)propertySnapshot.AddedBaseDamage / Duration;
            }

            return 0;
        }

        public override double Expire()
        {
            return 0;
        }
    }
}

public struct SkillPropertySnapshot
{
    public SkillPropertySnapshot()
    {
    }

    public double AddedBaseDamage { get; init; } = 0.0;
    public double AddedBaseDamageMultiplier { get; init; } = 0.0;
    public double Multiplier { get; init; } = 0.0;

    public static SkillPropertySnapshot operator +(SkillPropertySnapshot left, SkillPropertySnapshot right)
    {
        return new SkillPropertySnapshot
        {
            AddedBaseDamage = left.AddedBaseDamage + right.AddedBaseDamage,
            Multiplier = left.Multiplier + right.Multiplier,
            AddedBaseDamageMultiplier = left.AddedBaseDamageMultiplier + right.AddedBaseDamageMultiplier,
        };
    }
}