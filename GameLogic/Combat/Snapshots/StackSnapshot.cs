using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;
using static GameData.src.Shared.Duration;

namespace GameLogic.Combat.Snapshots
{
    public struct StackSnapshot
    {
        public string? EffectId;
        public int StackCount;
        public int StacksPerApplication;
        public int MaxStacks;
        public StackType StackType;
        public bool ConsumeStacks;
        public StackRefreshMode RefreshMode;

        public static StackSnapshot FromStrategy(IStackStrategy strategy)
        {
            if (strategy is StackDefault defaultStrategy)
            {
                return new StackSnapshot
                {
                    StackType = StackType.Default,
                    StackCount = 0,
                    StacksPerApplication = defaultStrategy.StacksPerApplication,
                    MaxStacks = defaultStrategy.MaxStacks,
                    RefreshMode = defaultStrategy.RefreshMode,
                };
            }
            else if (strategy is StackFromEffect fromEffectStrategy)
            {
                return new StackSnapshot
                {
                    StackType = StackType.FromEffect,
                    EffectId = fromEffectStrategy.EffectId,
                    ConsumeStacks = fromEffectStrategy.ConsumeStacks,
                };
            }

            throw new InvalidOperationException($"Invalid stack strategy: {strategy.GetType().Name}");
        }
    }

    public enum StackType
    {
        FromEffect, Default
    }
}