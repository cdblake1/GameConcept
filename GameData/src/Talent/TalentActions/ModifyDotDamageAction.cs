using GameData.src.Effect.Stack;
using GameData.src.Effect.Talent;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill.SkillStep;

namespace GameData.src.Talent.TalentActions
{
    public record ModifyDotDamageAction : ITalentAction
    {
        public required string SkillId { get; init; }
        public ScalarOperation? BaseDamage { get; init; }
        public AttackKind AttackKind => AttackKind.Dot;

        public DotDamageStep.TimingKind? Timing { get; init; }

        public DurationOperation? Duration { get; init; }

        public ScalarOperation? Frequency { get; init; }

        public IStackStrategy? StackStrategy { get; init; }

        public DamageTypeCollectionModifier? DamageTypes { get; init; }

        public bool? Crit { get; init; }
    }
}