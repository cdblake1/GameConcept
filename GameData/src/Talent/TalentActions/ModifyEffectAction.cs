using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Talent.TalentActions
{
    public class ModifyEffectAction : ITalentAction
    {
        public required string EffectId { get; init; }
        public DurationOperation? Duration { get; init; }
        public StackDefaultOperation? StackStrategy { get; init; }
        public ModifierCollectionModifier? Modifiers { get; init; }
    }
}