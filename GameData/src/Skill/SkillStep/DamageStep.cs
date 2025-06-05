using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;

namespace GameData.src.Skill.SkillStep
{
    public abstract record DamageStep : ISkillStep
    {
        public required AttackKind Kind { get; init; }

        public required IReadOnlyList<DamageType> DamageTypes { get; init; }

        public required int BaseDamage { get; init; }

        public required bool Crit { get; init; }

        public required ScaleCoefficient ScaleCoefficient { get; init; }
    }

    public sealed record HitDamageStep : DamageStep
    {
        public StackFromEffect? StackFromEffect { get; init; }
    }

    public sealed record DotDamageStep : DamageStep
    {
        public required Duration Duration { get; init; }

        public required int Frequency { get; init; }

        public required TimingKind Timing { get; init; }

        public required IStackStrategy Stacking { get; init; }

        public enum TimingKind { StartTurn, EndTurn }
    }
}