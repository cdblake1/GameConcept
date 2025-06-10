using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Talent.TalentActions
{
    public record ModifyHitDamageAction : ITalentAction
    {
        public required string SkillId { get; init; }
        public DamageTypeCollectionModifier? DamageTypes { get; init; }
        public ScalarOperation? MinBaseDamage { get; init; }
        public ScalarOperation? MaxBaseDamage { get; init; }
        public bool? Crit { get; init; }
    }
}