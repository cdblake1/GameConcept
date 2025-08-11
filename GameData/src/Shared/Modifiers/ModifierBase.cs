using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Shared.Modifiers
{
    public interface IModifier
    {
    }

    public sealed record SkillModifier(string SkillId, ScalarOpType Operation, int Value) : IModifier;
    public sealed record DamageModifier(DamageType DamageType, ScalarOpType Operation, int Value) : IModifier;
    public sealed record GlobalModifier(GlobalStat GlobalStat, ScalarOpType Operation, int Value) : IModifier;
    public sealed record AttackModifier(AttackType AttackType, ScalarOpType Operation, int Value) : IModifier;
    public sealed record WeaponModifier(WeaponType WeaponType, ScalarOpType Operation, int Value) : IModifier;
    public sealed record StatModifier(StatKind StatKind, float Value) : IModifier;
}