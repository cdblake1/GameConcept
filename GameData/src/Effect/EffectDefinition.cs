using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Modifiers;

namespace GameData.src.Effect
{
    public record EffectDefinition
    {
        public required string Id { get; init; }

        public required EffectCategory Category { get; init; }

        public required Duration Duration { get; init; }

        public required IStackStrategy StackStrategy { get; init; }

        public required IReadOnlyList<IModifier> Modifiers { get; init; }
    }
}