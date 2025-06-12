using GameData.src.Skill;
using GameLogic.Combat.Snapshots.Steps;

namespace GameLogic.Combat.Snapshots
{
    public struct SkillSnapshot(
        SkillDefinition skillDefinition,
        int cost,
        int cooldown,
        List<ApplyEffectSnapshot>? applyEffects,
        List<DamageStepSnapshot>? damageEffects,
        List<DotDamageStepSnapshot>? dotEffects
        )
    {
        public readonly SkillDefinition skillDefinition = skillDefinition;
        public int Cost = cost;
        public int Cooldown = cooldown;
        public List<ApplyEffectSnapshot> ApplyEffects = applyEffects ?? [];
        public List<DamageStepSnapshot> DamageSteps = damageEffects ?? [];
        public List<DotDamageStepSnapshot> DotEffects = dotEffects ?? [];
        public ActivationRequirement? ActivationRequirement;
    }
}