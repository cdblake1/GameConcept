using GameData.src.Effect.Stack;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;
using Infrastructure.Json.Dto.Skill;

namespace Infrastructure.Json.Mappers
{
    public static class SkillMappers
    {
        public static SkillDefinition ToDomain(this SkillDto dto)
        {
            return new SkillDefinition()
            {
                Cooldown = dto.cooldown,
                Cost = dto.cost,
                ActivationRequirement = dto.activation_req?.ToDomain(),
                Effects = [.. dto.effects.Select(e => e.ToDomain())],
                Id = dto.id,
                Presentation = dto.presentation.ToDomain()
            };
        }

        public static ActivationRequirement ToDomain(this ActivationRequirementDto dto)
        {
            return new ActivationRequirement()
            {
                ActivationKind = dto.type.ToDomain(),
                Count = dto.count,
                FromEffect = dto.id
            };
        }

        public static ISkillStep ToDomain(this SkillStepDto dto)
        {
            return dto switch
            {
                HitDamageStepDto d => d.ToDomain(),
                DotDamageStepDto d => d.ToDomain(),
                ApplyEffectStepDto d => d.ToDomain(),
                _ => throw new InvalidOperationException($"damage step not implemented: {dto.GetType().Name}")
            };
        }

        public static ApplyEffectStep ToDomain(this ApplyEffectStepDto dto)
        => new() { Effect = dto.effect };
        public static HitDamageStep ToDomain(this HitDamageStepDto dto)
        => new()
        {
            BaseDamage = dto.base_damage,
            DamageTypes = [.. dto.damage_types.Select(d => d.ToDomain())],
            Kind = dto.attack_kind.ToDomain(),
            StackFromEffect = dto.stack_from_effect?.ToDomain() as StackFromEffect,
            Crit = dto.crit,
            ScaleCoefficient = dto.scale_coef.ToDomain()
        };

        public static DotDamageStep ToDomain(this DotDamageStepDto dto)
        => new()
        {
            BaseDamage = dto.base_damage,
            DamageTypes = [.. dto.damage_types.Select(d => d.ToDomain())],
            Duration = dto.duration.ToDomain(),
            Frequency = dto.frequency,
            Kind = dto.attack_kind.ToDomain(),
            Stacking = dto.stacking?.ToDomain() ?? new StackDefault() { MaxStacks = 1, RefreshMode = StackRefreshMode.ResetTime, StacksPerApplication = 1 },
            Timing = dto.timing.ToDomain(),
            Crit = dto.crit,
            ScaleCoefficient = dto.scale_coef.ToDomain()
        };

        public static ActivationRequirement.Kind ToDomain(this ActivationRequirementDto.ActivationKind dto) => dto switch
        {
            ActivationRequirementDto.ActivationKind.effect_present => ActivationRequirement.Kind.EffectPresent,
            ActivationRequirementDto.ActivationKind.hp_pct_below => ActivationRequirement.Kind.HpBelowPercentage,
            _ => throw new InvalidOperationException($"Activation Kind not implemented: {dto}")
        };

        public static DotDamageStep.TimingKind ToDomain(this DotDamageStepDto.TimingKind dto)
        => dto switch
        {
            DotDamageStepDto.TimingKind.start_turn => DotDamageStep.TimingKind.StartTurn,
            DotDamageStepDto.TimingKind.end_turn => DotDamageStep.TimingKind.EndTurn,
            _ => throw new InvalidOperationException($"timing kind not implemented {dto}")
        };
    }
}