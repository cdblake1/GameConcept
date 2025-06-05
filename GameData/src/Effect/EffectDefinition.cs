using GameData.src.Effect.Stack;
using GameData.src.Effect.Status;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers;

namespace GameData.src.Effect
{
    public record EffectDefinition
    {
        public required string Id { get; init; }

        public required Kind Category { get; init; }

        public required Duration Duration { get; init; }

        public required IStackStrategy Stacking { get; init; }

        public required IReadOnlyList<ScalarModifierBase> Modifiers { get; init; }

        public required int Leech { get; init; }

        public required IReadOnlyList<DamageType> DamageTypes { get; init; }

        public required IReadOnlyList<IStatus> ApplyStatus { get; init; }

        public enum Kind { Buff, Debuff }
    }
}