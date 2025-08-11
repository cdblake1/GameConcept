using GameData.src.Effect.Stack;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill;
using GameData.src.Talent.TalentActions;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Dto.Effect;
using Infrastructure.Json.Dto.Skill;
using Infrastructure.Json.Dto.Skill.SkillStep;
using Infrastructure.Json.Dto.Talent;
using Infrastructure.Json.Mappers;
using Xunit;
using Xunit.Sdk;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class TalentMapperTests
    {
        [Fact]
        public void CanMapModifyHitActionDto()
        {
            var dto = new ModifyHitDamageActionDto("test_skill")
            {
                crit = true,
                damage_types = new DamageTypeCollectionOperationDto(CollectionOperationDto.add, [DamageTypeDto.bleed]),
                max_base_damage = new(1, 1, 1, 1),
                min_base_damage = new(1, 1, 1, 1),
            };

            var modifyHitAction = dto.ToDomain();

            // Assert
            Assert.NotNull(modifyHitAction);
            Assert.Equal("test_skill", modifyHitAction.SkillId);
            Assert.NotNull(modifyHitAction.MinBaseDamage);
            Assert.NotNull(modifyHitAction.MaxBaseDamage);
            Assert.NotNull(modifyHitAction.DamageTypes);
            Assert.NotNull(modifyHitAction.Crit);

            Assert.True(modifyHitAction.Crit);
            Assert.Equal(dto.max_base_damage.override_value, modifyHitAction.MaxBaseDamage.OverrideValue);
            Assert.Equal(dto.max_base_damage.scale_added, modifyHitAction.MaxBaseDamage.ScaleAdded);
            Assert.Equal(dto.max_base_damage.scale_empowered, modifyHitAction.MaxBaseDamage.ScaleEmpowered);
            Assert.Equal(dto.max_base_damage.scale_increased, modifyHitAction.MaxBaseDamage.ScaleIncreased);
        }

        [Fact]
        public void CanMapModifyDotDamageDto()
        {
            var dto = new ModifyDotDamageActionDto("test_skill")
            {
                crit = false,
                damage_types = new DamageTypeCollectionOperationDto(CollectionOperationDto.add, [DamageTypeDto.bleed]),
                duration = new DurationOperationDto()
                {
                    turns = new(1, 1, 1, 1)
                },
                frequency = new(1, 1, 1, 1),
                max_base_damage = new(1, 1, 1, 1),
                min_base_damage = new(1, 1, 1, 1),
                stack_strategy = new(new(1, 1, 1, 1), new(1, 1, 1, 1))
            };

            var mdda = dto.ToDomain();
            // Assert
            Assert.NotNull(mdda);
            Assert.NotNull(mdda.Crit);
            Assert.NotNull(mdda.DamageTypes);
            Assert.NotNull(mdda.Duration);
            Assert.NotNull(mdda.Frequency);
            Assert.NotNull(mdda.MaxBaseDamage);
            Assert.NotNull(mdda.MinBaseDamage);
            Assert.NotNull(mdda.StackStrategy);

            Assert.False(mdda.Crit);
            Assert.Single(mdda.DamageTypes.Operation.Items);
            Assert.Equal(CollectionOperationKind.Add, mdda.DamageTypes.Operation.Operation);

            Assert.Equal(1, mdda.Frequency.ScaleAdded);

            Assert.Equal(1, mdda.MaxBaseDamage.ScaleAdded);

            Assert.Equal(1, mdda.MinBaseDamage.ScaleAdded);

            Assert.Equal(1, mdda.StackStrategy.Maxstacks?.OverrideValue);
            Assert.Equal(1, mdda.StackStrategy.Maxstacks?.ScaleAdded);
            Assert.Equal(1, mdda.StackStrategy.Maxstacks?.ScaleEmpowered);
            Assert.Equal(1, mdda.StackStrategy.Maxstacks?.ScaleIncreased);
            Assert.Equal(1, mdda.StackStrategy.StacksPerApplication?.OverrideValue);
            Assert.Equal(1, mdda.StackStrategy.StacksPerApplication?.ScaleAdded);
            Assert.Equal(1, mdda.StackStrategy.StacksPerApplication?.ScaleEmpowered);
            Assert.Equal(1, mdda.StackStrategy.StacksPerApplication?.ScaleIncreased);
        }

        [Fact]
        public void CanMapModifySkillActionDto()
        {
            var dto = new ModifySkillActionDto("test_skill")
            {
                activation_requirement = new()
                {
                    count = 1,
                    type = Dto.Skill.ActivationRequirementDto.ActivationKind.effect_present,
                    effect_id = "test_effect"
                },
                cooldown = new(1, 1, 1, 1),
                cost = new(1, 1, 1, 1),
            };

            var skillAction = dto.ToDomain();

            Assert.NotNull(skillAction.ActivationRequirement);
            Assert.NotNull(skillAction.Cooldown);
            Assert.NotNull(skillAction.Cost);

            Assert.Equal(1, skillAction.ActivationRequirement.Count);
            Assert.Equal(ActivationRequirementType.EffectPresent, skillAction.ActivationRequirement.ActivationKind);
            Assert.Equal(dto.activation_requirement.effect_id, skillAction.ActivationRequirement.EffectId);

            Assert.Equal(dto.cooldown.scale_added, skillAction.Cooldown.ScaleAdded);
            Assert.Equal(dto.cooldown.scale_empowered, skillAction.Cooldown.ScaleEmpowered);
            Assert.Equal(dto.cooldown.scale_increased, skillAction.Cooldown.ScaleIncreased);
            Assert.Equal(dto.cooldown.override_value, skillAction.Cooldown.OverrideValue);

            Assert.Equal(dto.cost.scale_added, skillAction.Cost.ScaleAdded);
            Assert.Equal(dto.cost.scale_empowered, skillAction.Cost.ScaleEmpowered);
            Assert.Equal(dto.cost.scale_increased, skillAction.Cost.ScaleIncreased);
            Assert.Equal(dto.cost.override_value, skillAction.Cost.OverrideValue);
        }

        [Fact]
        public void CanMapModifyEffectDto()
        {
            var dto = new ModifyEffectActionDto("test_effect")
            {
                duration = new DurationOperationDto()
                {
                    turns = new ScalarOperationDto(1, 1, 1, 1)
                },
                modifiers = new ModifierCollectionOperationDto(CollectionOperationDto.add, [new DamageModifierDto(DamageTypeDto.physical, ScalarOpTypeDto.added, 1)])
            };

            var me = dto.ToDomain();

            // Basic null checks
            Assert.NotNull(me);
            Assert.NotNull(me.Duration);
            Assert.NotNull(me.Modifiers);
            Assert.NotNull(me.Duration.Turns);

            // Verify EffectId
            Assert.Equal("test_effect", me.EffectId);

            // Verify Duration properties
            Assert.Equal(1, me.Duration.Turns.OverrideValue);
            Assert.Equal(1, me.Duration.Turns.ScaleAdded);
            Assert.Equal(1, me.Duration.Turns.ScaleEmpowered);
            Assert.Equal(1, me.Duration.Turns.ScaleIncreased);

            // Verify Modifiers
            Assert.Single(me.Modifiers.Operation.Items);
            Assert.Equal(CollectionOperationKind.Add, me.Modifiers.Operation.Operation);

            // Verify DamageModifier
            var damageMod = me.Modifiers.Operation.Items[0] as DamageModifier;
            Assert.NotNull(damageMod);
            Assert.Equal(DamageType.Physical, damageMod.DamageType);
            Assert.Equal(1, damageMod.Value);
        }

        [Fact]
        public void CanMapApplyEffectActionDto()
        {
            var aeDto = new ApplyEffectActionDto("test_effect")
            {
                from_skill = "asdf"
            };

            var aeDto2 = new ApplyEffectActionDto("test_effect_2")
            {
                global = true
            };

            var ae = aeDto.ToDomain();
            var ae2 = aeDto2.ToDomain();

            Assert.Equal(aeDto.effect_id, ae.EffectId);
            Assert.Equal(aeDto2.effect_id, ae2.EffectId);

            Assert.Equal(aeDto.global, ae.Global);
            Assert.Equal(aeDto2.global, ae2.Global);

            Assert.Equal(aeDto.from_skill, ae.SkillId);
            Assert.Equal(aeDto2.from_skill, ae2.SkillId);

        }

        [Fact]
        public void CanMapAddHitDamageActionDto()
        {
            var dto = new AddHitDamageActionDto("test_skill", new HitDamageStepDto(
                damage_types: DamageTypeDto.bleed,
                weapon_type: WeaponTypeDto.melee,
                min_base_damage: 1,
                max_base_damage: 1,
                crit: true,
                scale_properties: new(1, 1, 1)
            ));

            var ah = dto.ToDomain();

            Assert.Equal(DamageType.Bleed, ah.HitDamage.DamageType);
            Assert.Equal(WeaponType.Melee, ah.HitDamage.WeaponType);
            Assert.Equal(dto.hit_damage.min_base_damage, ah.HitDamage.MinBaseDamage);
            Assert.True(ah.HitDamage.Crit);
            Assert.Equal(dto.hit_damage.scale_properties.scale_added, ah.HitDamage.ScaleProperties.ScaleAdded);
            Assert.Equal(dto.hit_damage.scale_properties.scale_increased, ah.HitDamage.ScaleProperties.ScaleIncreased);
            Assert.Equal(dto.hit_damage.scale_properties.scale_speed, ah.HitDamage.ScaleProperties.ScaleSpeed);
        }

        [Fact]
        public void CanMapAddDotDamageActionDto()
        {
            var dto = new AddDotDamageActionDto("test_skill", new(
                damage_types: DamageTypeDto.burn,
                weapon_type: WeaponTypeDto.melee,
                min_base_damage: 1,
                max_base_damage: 1,
                crit: false,
                duration: new PermanentDurationDto() { permanent = true },
                frequency: 1,
                stack_strategy: new StackDefaultDto()
                {
                    max_stacks = 1,
                    refresh_mode = StackStrategyRefreshMode.add_time,
                    stacks_per_application = 1
                },
                scale_properties: new SkillScalePropertiesDto(1, 1, 1)
            ));

            var dotAction = dto.ToDomain();

            Assert.NotNull(dotAction);

            Assert.Equal(dto.skill_id, dotAction.SkillId);
            Assert.Equal(DamageType.Burn, dotAction.DotDamage.DamageType);
            Assert.Equal(WeaponType.Melee, dotAction.DotDamage.WeaponType);
            Assert.Equal(dto.dot_damage.min_base_damage, dotAction.DotDamage.MinBaseDamage);
            Assert.Equal(dto.dot_damage.max_base_damage, dotAction.DotDamage.MaxBaseDamage);
            Assert.Equal(dto.dot_damage.crit, dotAction.DotDamage.Crit);
            Assert.True(dotAction.DotDamage.Duration.Kind == GameData.src.Shared.DurationKind.Permanent);
            Assert.Equal(dto.dot_damage.frequency, dotAction.DotDamage.Frequency);

            var stackStrategy = dotAction.DotDamage.StackStrategy as StackDefault;
            Assert.NotNull(stackStrategy);
            Assert.Equal(1, stackStrategy.MaxStacks);
            Assert.Equal(1, stackStrategy.StacksPerApplication);
            Assert.Equal(StackRefreshMode.AddTime, stackStrategy.RefreshMode);

            Assert.Equal(1, dotAction.DotDamage.ScaleProperties.ScaleAdded);
            Assert.Equal(1, dotAction.DotDamage.ScaleProperties.ScaleIncreased);
            Assert.Equal(1, dotAction.DotDamage.ScaleProperties.ScaleSpeed);
        }

        [Fact]
        public void CanMapTalentDto()
        {
            var dto = new TalentDto()
            {
                actions = [
                    new ModifyEffectActionDto("test_effect") {
                        duration = new DurationOperationDto()
                        {
                            turns = new(1,1,1,1)
                        },
                        modifiers = new ModifierCollectionOperationDto(CollectionOperationDto.add, []),
                        stack_strategy = new StackDefaultModifierDto(new(1,1,1,1), null)
                    }
                ],
                id = "test_talent",
                presentation = new()
                {
                    description = "test description",
                    name = "test name"
                }
            };

            var talent = dto.ToDomain();

            Assert.Equal(dto.actions.Length, talent.Actions.Count);

            Assert.NotNull(talent.Actions);
            Assert.Single(talent.Actions);

            var action = talent.Actions[0] as ModifyEffectAction;

            Assert.NotNull(action);
            Assert.NotNull(action.Duration);
            Assert.NotNull(action.Modifiers);
            Assert.NotNull(action.StackStrategy);
            Assert.Equal("test_effect", action.EffectId);

            Assert.Equal(1, action.Duration.Turns?.OverrideValue);
            Assert.Equal(1, action.Duration.Turns?.ScaleAdded);
            Assert.Equal(1, action.Duration.Turns?.ScaleEmpowered);
            Assert.Equal(1, action.Duration.Turns?.ScaleIncreased);

            Assert.Empty(action.Modifiers.Operation.Items);
            Assert.Equal(CollectionOperationKind.Add, action.Modifiers.Operation.Operation);

            Assert.Equal(dto.id, talent.Id);

            Assert.Equal(dto.presentation.description, talent.Presentation.Description);
            Assert.Equal(dto.presentation.name, talent.Presentation.Name);
        }
    }
}