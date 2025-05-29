public class DefaultMobAttack : Skill
{
    public DefaultMobAttack() : base()
    {
    }

    public override string Name => "Scratch";
    public override string Description => "A basic attack that deals damage to the target.";

    public override IReadOnlyList<Effect> Apply(SkillPropertySnapshot snapshot)
    {
        var returnEffects = new List<Effect>();

        foreach (var effect in this.Effects)
        {
            if (effect is SkillModifier effectModifier)
            {
                snapshot += effectModifier.Apply();
            }
        }

        foreach (var effect in this.Effects)
        {
            if (effect is StatusEffect effectModifier)
            {
                effectModifier.ProvideSnapshot(snapshot);
                returnEffects.Add(effectModifier);
            }
        }

        returnEffects.Add(new DefaultMobAttackEffect(snapshot));

        return returnEffects;
    }

    public class DefaultMobAttackEffect : DamageEffect
    {
        public override string Name => "Scratch";
        public override string Description => "A basic attack that deals damage to the target.";

        public DefaultMobAttackEffect(SkillPropertySnapshot snapshot) : base(snapshot)
        {
        }
        public override double Damage()
        {
            return this.propertySnapshot.AddedBaseDamage * this.propertySnapshot.AddedBaseDamageMultiplier * this.propertySnapshot.Multiplier;
        }
    }
}