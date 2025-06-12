using GameData.src.Effect;
using GameData.src.Shared.Modifiers;
using GameData.src.Skill;
using GameLogic.Combat.Snapshots.Steps;

namespace GameLogic.Combat.Snapshots
{
    public struct EffectSnapshot
    {
        private const string dotIdentifier = "DotEffect";
        public List<IModifier> Modifiers;
        public DamageSnapshot? Damage;
        public required string EffectId;
        public required DurationSnapshot Duration;
        public required StackSnapshot StackStrategy;
        public required EffectCategory Category;

        public static EffectSnapshot FromEffect(EffectDefinition effect)
        {
            List<IModifier> modifiers = [];
            if (effect.Modifiers.Count > 0)
            {
                for (int i = 0; i < effect.Modifiers.Count; i++)
                {
                    modifiers.Add(effect.Modifiers[i]);
                }
            }

            return new()
            {
                EffectId = effect.Id,
                Duration = DurationSnapshot.FromDuration(effect.Duration),
                StackStrategy = StackSnapshot.FromStrategy(effect.StackStrategy),
                Category = effect.Category,
                Modifiers = modifiers
            };
        }

        public static EffectSnapshot FromDot(SkillDefinition skill, DotDamageStepSnapshot step)
        {
            return new()
            {
                EffectId = dotIdentifier,
                Category = EffectCategory.Debuff,
                Duration = step.Duration,
                StackStrategy = step.StackStrategy,
                Damage = DamageSnapshotBuilder.BuildNew(skill, step)
            };
        }
    }
}