using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;
using Infrastructure.Json.Dto.Skill;
using Infrastructure.Json.Dto.Skill.SkillStep;

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
                EffectId = dto.effect_id
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

        public static HitDamageStep ToDomain(this HitDamageStepDto dto)
        => new(
            AttackType: dto.attack_type.ToDomain(),
            DamageType: dto.damage_types.ToDomain(),
            WeaponType: dto.weapon_type.ToDomain(),
            MinBaseDamage: dto.min_base_damage,
            MaxBaseDamage: dto.max_base_damage,
            Crit: dto.crit,
            ScaleProperties: dto.scale_properties.ToDomain(),
            StackFromEffect: dto.stack_from_effect?.ToDomain()
        );

        public static DotDamageStep ToDomain(this DotDamageStepDto dto)
        => new(
            AttackType: dto.attack_type.ToDomain(),
            DamageType: dto.damage_types.ToDomain(),
            WeaponType: dto.weapon_type.ToDomain(),
            MinBaseDamage: dto.min_base_damage,
            MaxBaseDamage: dto.max_base_damage,
            Crit: dto.crit,
            Duration: dto.duration.ToDomain(),
            Frequency: dto.frequency,
            StackStrategy: dto.stack_strategy.ToDomain(),
            ScaleProperties: dto.scale_properties.ToDomain()
        );

        public static ApplyEffectStep ToDomain(this ApplyEffectStepDto dto)
            => new(dto.effect_id);

        public static SkillScaleProperties ToDomain(this SkillScalePropertiesDto dto)
            => new(dto.scale_added, dto.scale_increased, dto.scale_speed);


        public static ActivationRequirementType ToDomain(this ActivationRequirementDto.ActivationKind dto) => dto switch
        {
            ActivationRequirementDto.ActivationKind.effect_present => ActivationRequirementType.EffectPresent,
            ActivationRequirementDto.ActivationKind.hp_pct_below => ActivationRequirementType.HpBelowPercentage,
            _ => throw new InvalidOperationException($"Activation Kind not implemented: {dto}")
        };
    }
}