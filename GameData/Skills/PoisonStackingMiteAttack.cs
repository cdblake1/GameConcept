
public class PoisonSwarmSkill : Skill
{
    public override string Name => "Poison Swarm";

    public override string Description => "A swarm of poisonous insects that attack the target, dealing damage over time.";

    public override IReadOnlyList<Effect> Apply(SkillPropertySnapshot snapshot)
    {
        return [
            new PoisonSwarmDamageEffect(snapshot),
            new PoisonSwarmStackEffect(snapshot)
        ];
    }

    public class PoisonSwarmDamageEffect : DamageEffect
    {
        public override string Name => nameof(PoisonSwarmDamageEffect);
        public override string Description => "Deals poison damage over time.";


        public PoisonSwarmDamageEffect(SkillPropertySnapshot snapshot) : base(snapshot)
        {
        }

        public override double Damage()
        {
            // Implement the logic to calculate poison damage based on the snapshot
            return this.propertySnapshot.AddedBaseDamage * this.propertySnapshot.AddedBaseDamageMultiplier * this.propertySnapshot.Multiplier;
        }
    }


    public class PoisonSwarmStackEffect : StatusEffect
    {
        public override string Name => nameof(PoisonSwarmStackEffect);
        public override string Description => "Stacking Poison Damage, dealing damage over time.";

        public override int Duration => 2;

        private int stacks;
        private readonly int maxStacks = 5;

        public int CurrentStacks => stacks;

        private int currentDuration = 0;

        public PoisonSwarmStackEffect(SkillPropertySnapshot snapshot) : base(snapshot)
        {
            this.stacks = 1;
        }

        public override double Tick()
        {
            if (currentDuration >= Duration)
            {
                return 0; // Effect expired or max stacks reached
            }

            currentDuration++;
            this.stacks = Math.Min(this.stacks + 1, maxStacks);

            return this.propertySnapshot.AddedBaseDamage * this.stacks;
        }

        public override double Expire()
        {
            throw new NotImplementedException();
        }
    }
}