using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Shared.Modifiers
{
    public interface IModifier
    {
    }

    public sealed record SkillModifier(string SkillId, ScalarOperation Operation) : IModifier;
    public sealed record DamageModifier(DamageType DamageType, ScalarOperation Operation) : IModifier;
    public sealed record GlobalModifier(GlobalStat GlobalStat, ScalarOperation Operation) : IModifier;
    public sealed record AttackModifier(AttackType AttackType, ScalarOperation Operation) : IModifier;
    public sealed record WeaponModifier(WeaponType WeaponType, ScalarOperation Operation) : IModifier;
    public sealed record StatModifier(StatKind StatKind, float Value) : IModifier;
}