using GameData.src.Effect.Talent;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Talent;
using GameData.src.Talent.TalentActions;
using GameData.src.Shared.Modifiers;

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

        public static ITalentAction ToDomain(this TalentActionBaseDto dto)
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
                ModifyHitDamageActionDto d => new ModifyHitDamageAction()
                {
                    BaseDamage = d.base_damage?.ToDomain(),
                    Crit = d.crit,
                    Skill = d.skill,
                    DamageTypes = d.damage_types?.ToDomain()
                },

                ModifyDotDamageActionDto d => new ModifyDotDamageAction()
                {
                    BaseDamage = d.base_damage?.ToDomain(),
                    Duration = d.duration?.ToDomain(),
                    Frequency = d.frequency?.ToDomain(),
                    StackStrategy = d.stacking?.ToDomain(),
                    Timing = d.timing?.ToDomain(),
                    SkillId = d.skill,
                    Crit = d.crit,
                    DamageTypes = d.damage_types?.ToDomain()
                },

                ModifySkillActionDto d => new ModifySkillAction()
                {
                    Skill = d.skill,
                    ActivationRequirement = d.activation_req?.ToDomain(),
                    Cooldown = d.cooldown?.ToDomain(),
                    Cost = d.cost?.ToDomain(),
                },

                ModifyEffectActionDto d => new ModifyEffectAction()
                {
                    Id = d.id,
                    DamageTypes = d.damage_types?.ToDomain(),
                    Duration = d.duration?.ToDomain(),
                    Leech = d.leech?.ToDomain(),
                    Stacking = d.stacking?.ToDomain(),
                    ApplyStatus = d.apply_status?.ToDomain(),
                    Modifiers = d.modifiers?.ToDomain(),
                },

                _ => throw new InvalidOperationException($"Talent Action not implemented: {dto.GetType().Name} in TalentMappers.ToDomain.")
            };
        }
    }
}