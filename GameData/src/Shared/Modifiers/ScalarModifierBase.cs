using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;

namespace GameData.src.Shared.Modifiers
{
    public abstract record ScalarModifierBase
    {
        public ScalarOperation Operation { get; }

        public ScalarModifierBase(ScalarOperation operation)
        {
            this.Operation = operation;
        }
    }

    public sealed record SkillModifier(string SkillId, ScalarOperation Operation) : ScalarModifierBase(Operation);
    public sealed record DamageModifer(DamageType DamageType, ScalarOperation Operation) : ScalarModifierBase(Operation);
    public sealed record AttackModifer(AttackKind AttackKind, ScalarOperation Operation) : ScalarModifierBase(Operation);
    public sealed record StatModifier(StatKind StatKind, ScalarOperation Operation) : ScalarModifierBase(Operation);
}