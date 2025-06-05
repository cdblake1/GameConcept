using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Effect.Talent
{
    public class ModifyEffectAction : ITalentAction
    {
        public required string Id { get; init; }

        public EffectDefinition.Kind? Category { get; init; }

        public DurationOperation? Duration { get; init; }

        public IStackStrategy? Stacking { get; init; }

        public ScalarOperation? Leech { get; init; }

        public DamageTypeCollectionModifier? DamageTypes { get; init; }

        public StatusCollectionModifier? ApplyStatus { get; init; }

        public ModifierCollectionModifier? Modifiers { get; init; }
    }
}