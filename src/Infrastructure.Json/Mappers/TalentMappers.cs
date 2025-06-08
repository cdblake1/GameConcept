using GameData.src.Effect.Talent;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Talent;
using GameData.src.Talent.TalentActions;

namespace Infrastructure.Json.Mappers
{
    public static class TalentMappers
    {
        public static TalentDefinition ToDomain(this TalentDto dto)
        {
            return new TalentDefinition()
            {
                Actions = [.. dto.actions.Select(a => a.ToDomain())],
                Id = dto.id,
                Presentation = dto.presentation.ToDomain()
            };
        }

        public static ModifyHitDamageAction ToDomain(this ModifyHitDamageActionDto dto)
        => new()
        {
            SkillId = dto.skill_id,
            Crit = dto.crit,
            DamageTypes = dto.damage_types?.ToDomain(),
            MaxBaseDamage = dto.max_base_damage?.ToDomain(),
            MinBaseDamage = dto.min_base_damage?.ToDomain()
        };

        public static ModifyDotDamageAction ToDomain(this ModifyDotDamageActionDto dto)
        => new()
        {
            SkillId = dto.skill_id,
            Crit = dto.crit,
            DamageTypes = dto.damage_types?.ToDomain(),
            Duration = dto.duration?.ToDomain(),
            Frequency = dto.frequency?.ToDomain(),
            MinBaseDamage = dto.min_base_damage?.ToDomain(),
            MaxBaseDamage = dto.max_base_damage?.ToDomain(),
            StackStrategy = dto.stack_strategy?.ToDomain()
        };

        public static ModifyEffectAction ToDomain(this ModifyEffectActionDto dto)
        => new()
        {
            EffectId = dto.effect_id,
            Duration = dto.duration?.ToDomain(),
            Modifiers = dto.modifiers?.ToDomain(),
            StackStrategy = dto.stack_strategy?.ToDomain(),
        };

        public static ApplyEffectAction ToDomain(this ApplyEffectActionDto dto)
        => new(dto.effect_id, dto.from_skill, dto.global);

        public static ModifySkillAction ToDomain(this ModifySkillActionDto dto)
        => new()
        {
            SkillId = dto.skill_id,
            Cooldown = dto.cooldown?.ToDomain(),
            Cost = dto.cost?.ToDomain(),
            ActivationRequirement = dto.activation_requirement?.ToDomain()
        };

        public static AddHitDamageAction ToDomain(this AddHitDamageActionDto dto)
        => new(dto.skill_id, dto.hit_damage.ToDomain());

        public static AddDotDamageAction ToDomain(this AddDotDamageActionDto dto)
        => new(dto.skill_id, dto.dot_damage.ToDomain());

        public static ITalentAction ToDomain(this ITalentActionDto dto)
        {
            if (dto is IValidatableEntity validate)
            {
                if (!validate.Validate())
                {
                    throw new InvalidOperationException($"Talent Action was invalid: {dto.GetType().Name}");
                }
            }

            return dto switch
            {
                ModifyHitDamageActionDto mhd => mhd.ToDomain(),
                ModifyDotDamageActionDto mdd => mdd.ToDomain(),
                ModifyEffectActionDto mea => mea.ToDomain(),
                ApplyEffectActionDto aea => aea.ToDomain(),
                ModifySkillActionDto msa => msa.ToDomain(),
                AddHitDamageActionDto ahd => ahd.ToDomain(),
                AddDotDamageActionDto add => add.ToDomain(),
                _ => throw new InvalidOperationException($"Talent Action not implemented: {dto.GetType().Name} in TalentMappers.ToDomain.")
            };
        }
    }
}