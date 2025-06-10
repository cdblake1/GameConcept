using GameData.src.Skill;
using GameLogic.Combat.Snapshots.Steps;

namespace GameLogic.Combat.Snapshots
{
    public struct SkillSnapshot(
        SkillDefinition skillDefinition,
        int cost,
        int cooldown,
        Memory<ApplyEffectSnapshot>? applyEffects,
        Memory<DamageStepSnapshot>? damageEffects,
        Memory<DotDamageStepSnapshot>? dotEffects
        )
    {
        public readonly SkillDefinition skillDefinition = skillDefinition;
        public int Cost = cost;
        public int Cooldown = cooldown;
        public Memory<ApplyEffectSnapshot> ApplyEffects = applyEffects ?? Memory<ApplyEffectSnapshot>.Empty;
        public Memory<DamageStepSnapshot> DamageSteps = damageEffects ?? Memory<DamageStepSnapshot>.Empty;
        public Memory<DotDamageStepSnapshot> DotEffects = dotEffects ?? Memory<DotDamageStepSnapshot>.Empty;
        public ActivationRequirement? ActivationRequirement;
    }
}