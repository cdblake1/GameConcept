using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;

namespace GameData.src.Skill.SkillStep
{
    public sealed record DotDamageStep(
        AttackType AttackType,
        IReadOnlyList<DamageType> DamageTypes,
        WeaponType WeaponType,
        int MinBaseDamage,
        int MaxBaseDamage,
        bool Crit,
        Duration Duration,
        int Frequency,
        IStackStrategy StackStrategy,
        SkillScaleProperties ScaleProperties
    ) : ISkillStep;
}