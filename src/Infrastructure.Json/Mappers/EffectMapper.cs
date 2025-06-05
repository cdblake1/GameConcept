using GameData.src.Effect;
using GameData.src.Effect.Stack;
using GameData.src.Effect.Status;
using GameData.src.Shared.Enums;
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
                    EffectBaseDto.Category.buff => EffectDefinition.Kind.Buff,
                    EffectBaseDto.Category.debuff => EffectDefinition.Kind.Debuff,
                    _ => throw new InvalidOperationException($"Effect Category not implemented {dto.category}")
                },
                Duration = dto.duration.ToDomain(),
                Id = dto.id,
                Leech = dto.leech ?? 1,
                Modifiers = dto.modifiers != null
                    ? dto.modifiers.Select(m => m.ToDomain()).ToList()
                    : Array.Empty<ScalarModifierBase>(),

                ApplyStatus = dto.apply_status != null
                    ? dto.apply_status.Select(s => s.ToDomain()).ToList()
                    : Array.Empty<IStatus>(),

                DamageTypes = dto.damage_types != null
                    ? dto.damage_types.Select(d => d.ToDomain()).ToList()
                    : Array.Empty<DamageType>(),

                Stacking = dto.stacking != null
                    ? dto.stacking.ToDomain()
                    : new StackDefault() { MaxStacks = 1, RefreshMode = StackRefreshMode.ResetTime, StacksPerApplication = 1 }
            };
        }

        public static IStatus ToDomain(this StatusBaseDto dto) => dto switch
        {
            StunStatusDto d => StunStatus.Create(d.duration.ToDomain()),
            _ => throw new InvalidOperationException($"status not implemented {dto.GetType().Name}")
        };

        public static IStackStrategy ToDomain(this StackBaseDto dto) => dto switch
        {
            StackDefaultDto d => new StackDefault() { MaxStacks = d.max_stacks, RefreshMode = d.refresh_mode.ToDomain(), StacksPerApplication = d.stacks_per_application },
            StackFromEffectDto d => new StackFromEffect() { ConsumeStacks = d.consume_stacks, FromEffect = d.from },
            _ => throw new InvalidOperationException($"Stack Strategy not implemented {dto.GetType().Name}")
        };

        public static StackRefreshMode ToDomain(this StackDefaultDto.RefreshMode dto) => dto switch
        {
            StackDefaultDto.RefreshMode.add_time => StackRefreshMode.AddTime,
            StackDefaultDto.RefreshMode.reset_time => StackRefreshMode.ResetTime,
            StackDefaultDto.RefreshMode.no_refresh => StackRefreshMode.NoRefresh,
            _ => throw new InvalidOperationException($"Refresh mode not implemented {dto}")
        };

        public static EffectDefinition.Kind ToDomain(this EffectBaseDto.Category dto)
        {
            return dto switch
            {
                EffectBaseDto.Category.buff => EffectDefinition.Kind.Buff,
                EffectBaseDto.Category.debuff => EffectDefinition.Kind.Debuff,
                _ => throw new InvalidOperationException($"effect category not implemented: {dto}")
            };
        }
    }
}