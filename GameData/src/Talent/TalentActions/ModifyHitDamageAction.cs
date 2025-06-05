using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.Modifiers;

namespace GameData.src.Effect.Talent
{
    public record ModifyHitDamageAction : ITalentAction
    {
        public AttackKind AttackKind { get; } = AttackKind.Hit;

        public required string Skill { get; init; }

        public bool? Crit { get; init; }

        public DamageTypeCollectionModifier? DamageTypes { get; init; }

        public ScalarOperation? BaseDamage { get; init; }
    }
}