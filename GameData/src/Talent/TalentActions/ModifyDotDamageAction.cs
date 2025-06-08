using GameData.src.Effect.Talent;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Talent.TalentActions
{
    public record ModifyDotDamageAction : ITalentAction
    {
        public required string SkillId { get; init; }
        public DamageTypeCollectionModifier? DamageTypes { get; init; }
        public ScalarOperation? MinBaseDamage { get; init; }
        public ScalarOperation? MaxBaseDamage { get; init; }
        public bool? Crit { get; init; }
        public DurationOperation? Duration { get; init; }
        public ScalarOperation? Frequency { get; init; }
        public StackDefaultOperation? StackStrategy { get; init; }
    }
}