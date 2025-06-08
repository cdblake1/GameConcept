using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Shared.Modifiers
{
    public interface IModifier
    {
    }

    public sealed record SkillModifier(string SkillId, ScalarOperation Operation) : IModifier;
    public sealed record StatModifier(StatKind StatKind, float Value) : IModifier;
}