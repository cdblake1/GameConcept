using GameData.src.Effect;
using GameData.src.Effect.Stack;
using GameData.src.Effect.Status;
using GameData.src.Shared.Modifiers;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Effect;

namespace Infrastructure.Json.Mappers
{
    public static class EffectMappers
    {
        public static EffectDefinition ToDomain(this EffectBaseDto dto)
        {
            return new EffectDefinition()
            {
                Category = dto.category switch
                {
                    EffectCategoryDto.buff => EffectCategory.Buff,
                    EffectCategoryDto.debuff => EffectCategory.Debuff,
                    _ => throw new InvalidOperationException($"Effect Category not implemented {dto.category}")
                },
                Duration = dto.duration.ToDomain(),
                Id = dto.id,
                Modifiers = dto.modifiers != null
                    ? dto.modifiers.Select(m => m.ToDomain()).ToList()
                    : Array.Empty<IModifier>(),
                StackStrategy = dto.stack_strategy != null
                    ? dto.stack_strategy.ToDomain()
                    : new StackDefault() { MaxStacks = 1, RefreshMode = StackRefreshMode.ResetTime, StacksPerApplication = 1 }
            };
        }

        public static IStatus ToDomain(this StatusBaseDto dto) => dto switch
        {
            StunStatusDto d => StunStatus.Create(d.duration.ToDomain()),
            _ => throw new InvalidOperationException($"status not implemented {dto.GetType().Name}")
        };

        public static IStackStrategy ToDomain(this StackStrategyBaseDto dto) => dto switch
        {
            StackDefaultDto d => new StackDefault() { MaxStacks = d.max_stacks, RefreshMode = d.refresh_mode.ToDomain(), StacksPerApplication = d.stacks_per_application },
            StackFromEffectDto d => d.ToDomain(),
            _ => throw new InvalidOperationException($"Stack Strategy not implemented {dto.GetType().Name}")
        };

        public static StackFromEffect ToDomain(this StackFromEffectDto dto)
        => new StackFromEffect() { ConsumeStacks = dto.consume_stacks, EffectId = dto.effect_id };

        public static StackRefreshMode ToDomain(this StackStrategyRefreshMode dto) => dto switch
        {
            StackStrategyRefreshMode.add_time => StackRefreshMode.AddTime,
            StackStrategyRefreshMode.reset_time => StackRefreshMode.ResetTime,
            StackStrategyRefreshMode.no_refresh => StackRefreshMode.NoRefresh,
            _ => throw new InvalidOperationException($"Refresh mode not implemented {dto}")
        };

        public static EffectCategory ToDomain(this EffectCategoryDto dto)
        {
            return dto switch
            {
                EffectCategoryDto.buff => EffectCategory.Buff,
                EffectCategoryDto.debuff => EffectCategory.Debuff,
                _ => throw new InvalidOperationException($"effect category not implemented: {dto}")
            };
        }
    }
}