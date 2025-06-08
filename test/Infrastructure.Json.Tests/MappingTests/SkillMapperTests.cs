using GameData.src.Effect.Stack;
using GameData.src.Shared.Enums;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Effect;
using Infrastructure.Json.Dto.Skill;
using Infrastructure.Json.Dto.Skill.SkillStep;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class SkillMapperTests
    {
        [Fact]
        public void CanMapApplyEffectStepDto()
        {
            var skillStepDto = new ApplyEffectStepDto("test_effect");

            var applyEffect = skillStepDto.ToDomain();

            Assert.NotNull(applyEffect);
            Assert.Equal(skillStepDto.effect_id, applyEffect.EffectId);
        }

        [Fact]
        public void CanMapHitDamageStepDto()
        {
            var hdDto = new HitDamageStepDto(
                damage_types: [DamageTypeDto.physical],
                weapon_type: WeaponTypeDto.melee,
                min_base_damage: 1,
                max_base_damage: 1,
                crit: true,
                scale_properties: new(1, 1, 1)
            );

            var hitDamageStep = hdDto.ToDomain();

            Assert.NotNull(hitDamageStep);

            Assert.Equal(hdDto.min_base_damage, hitDamageStep.MinBaseDamage);
            Assert.Equal(hdDto.max_base_damage, hitDamageStep.MaxBaseDamage);

            Assert.Equal(WeaponType.Melee, hitDamageStep.WeaponType);

            Assert.True(hitDamageStep.Crit);

            Assert.Equal(1, hitDamageStep.ScaleProperties.ScaleAdded);
            Assert.Equal(1, hitDamageStep.ScaleProperties.ScaleIncreased);
            Assert.Equal(1, hitDamageStep.ScaleProperties.ScaleSpeed);
        }

        [Fact]
        public void CanMapDotDamageStepDto()
        {
            var dotDto = new DotDamageStepDto(
                damage_types: [DamageTypeDto.physical],
                weapon_type: WeaponTypeDto.melee,
                min_base_damage: 1,
                max_base_damage: 10,
                crit: false,
                duration: new PermanentDurationDto() { permanent = true },
                frequency: 1,
                stack_strategy: new StackDefaultDto() { max_stacks = 1, refresh_mode = StackStrategyRefreshMode.reset_time, stacks_per_application = 1 },
                scale_properties: new(1, 1, 1)
            );

            var dotDamageStep = dotDto.ToDomain();
            Assert.NotNull(dotDamageStep);
            Assert.Equal(WeaponType.Melee, dotDamageStep.WeaponType);
            Assert.Equal(dotDto.min_base_damage, dotDamageStep.MinBaseDamage);
            Assert.Equal(dotDto.max_base_damage, dotDamageStep.MaxBaseDamage);
            Assert.Equal(dotDto.crit, dotDamageStep.Crit);
            Assert.Single(dotDamageStep.DamageTypes);
            Assert.Equal(DamageType.Physical, dotDamageStep.DamageTypes[0]);
            Assert.True(dotDamageStep.Duration.Type == GameData.src.Shared.Duration.Kind.Permanent);
            Assert.Equal(dotDto.frequency, dotDamageStep.Frequency);
            Assert.NotNull(dotDamageStep.StackStrategy);
            Assert.Equal(((StackDefaultDto)dotDto.stack_strategy).max_stacks, ((StackDefault)dotDamageStep.StackStrategy).MaxStacks);
            Assert.Equal(StackRefreshMode.ResetTime, ((StackDefault)dotDamageStep.StackStrategy).RefreshMode);
            Assert.Equal(((StackDefaultDto)dotDto.stack_strategy).stacks_per_application, ((StackDefault)dotDamageStep.StackStrategy).StacksPerApplication);
            Assert.Equal(dotDto.scale_properties.scale_increased, dotDamageStep.ScaleProperties.ScaleIncreased);
            Assert.Equal(dotDto.scale_properties.scale_added, dotDamageStep.ScaleProperties.ScaleAdded);
            Assert.Equal(dotDto.scale_properties.scale_speed, dotDamageStep.ScaleProperties.ScaleSpeed);
        }

        [Fact]
        public void CanMapSkillDto()
        {
            var skillDto = new SkillDto()
            {
                cooldown = 1,
                cost = 1,
                effects = [new HitDamageStepDto(
                    damage_types: [DamageTypeDto.true_damage],
                    weapon_type: WeaponTypeDto.spell,
                    min_base_damage: 1,
                    max_base_damage: 1,
                    crit: true,
                    scale_properties: new(1,1,1)
                ) { }],
                id = "test_skill",
                presentation = new PresentationDto()
                {
                    name = "Test Effect",
                    description = "test description"
                },
                activation_req = new ActivationRequirementDto()
                {
                    count = 1,
                    type = ActivationRequirementDto.ActivationKind.effect_present,
                }
            };

            var skillDef = skillDto.ToDomain();

            Assert.NotNull(skillDef);
            Assert.Equal(skillDto.cooldown, skillDef.Cooldown);
            Assert.Equal(skillDto.cost, skillDef.Cost);

            Assert.NotNull(skillDef.Effects);
            Assert.Single(skillDef.Effects);

            var hitDamageStep = skillDef.Effects[0] as HitDamageStep;
            Assert.NotNull(hitDamageStep);
            Assert.Equal(AttackType.Hit, hitDamageStep.AttackType);
            Assert.True(hitDamageStep.Crit);
            Assert.NotNull(hitDamageStep.DamageTypes);
            Assert.Single(hitDamageStep.DamageTypes);
            Assert.Equal(1, hitDamageStep.MinBaseDamage);
            Assert.Equal(1, hitDamageStep.MaxBaseDamage);
            Assert.Equal(1, hitDamageStep.ScaleProperties.ScaleAdded);
            Assert.Equal(1, hitDamageStep.ScaleProperties.ScaleIncreased);
            Assert.Equal(1, hitDamageStep.ScaleProperties.ScaleSpeed);
            Assert.Equal(DamageType.TrueDamage, hitDamageStep.DamageTypes[0]);

            Assert.Equal(skillDto.id, skillDef.Id);

            Assert.NotNull(skillDef.Presentation);
            Assert.Equal(skillDto.presentation.name, skillDef.Presentation.Name);
            Assert.Equal(skillDto.presentation.description, skillDef.Presentation.Description);

            Assert.NotNull(skillDef.ActivationRequirement);
            Assert.Equal(skillDto.activation_req.count, skillDef.ActivationRequirement.Count);
            Assert.Equal(ActivationRequirementType.EffectPresent, skillDef.ActivationRequirement.ActivationKind);
        }
    }
}