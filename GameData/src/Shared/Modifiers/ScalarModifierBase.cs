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

    public sealed record SkillModifier(ScalarOperation Operation) : ScalarModifierBase(Operation);
    public sealed record DamageModifer(ScalarOperation Operation) : ScalarModifierBase(Operation);
    public sealed record AttackModifer(ScalarOperation Operation) : ScalarModifierBase(Operation);
    public sealed record StatModifier(ScalarOperation Operation) : ScalarModifierBase(Operation);
}