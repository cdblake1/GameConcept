using GameData.src.Effect.Stack;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Skill.SkillStep;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Effect;
using Infrastructure.Json.Dto.Skill;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class SkillMapperTests
    {
        [Fact]
        public void CanMapSkillStepDto()
        {
            var skillStepDto = new ApplyEffectStepDto()
            {
                effect = "test_effect"
            };

            var applyStatusEffect = skillStepDto.ToDomain() as ApplyEffectStep;
            Assert.NotNull(applyStatusEffect);
            Assert.Equal(skillStepDto.effect, applyStatusEffect.Effect);
        }

        [Fact]
        public void CanMapHitDamageStepDto()
        {
            var hdDto = new HitDamageStepDto()
            {
                base_damage = 1,
                damage_types = [DamageTypeDto.bleed],
                crit = true,
                stack_from_effect = new()
                {
                    consume_stacks = false,
                    from = "test_effect",
                },
                scale_coef = new ScaleCoefficientDto()
                {
                    scalar_operation = new(Dto.Common.Operations.ScalarOperationDto.Operation.add, 1),
                    stat = StatDto.melee_damage
                }
            };

            var hitDamageStep = hdDto.ToDomain();

            Assert.NotNull(hitDamageStep);
            Assert.Equal(hdDto.base_damage, hitDamageStep.BaseDamage);
            Assert.NotNull(hitDamageStep.DamageTypes);
            Assert.Single(hitDamageStep.DamageTypes);
            Assert.Equal(DamageType.Bleed, hitDamageStep.DamageTypes.First());
            Assert.Equal(hdDto.crit, hitDamageStep.Crit);
            Assert.NotNull(hitDamageStep.StackFromEffect);
            Assert.Equal(hdDto.stack_from_effect.consume_stacks, hitDamageStep.StackFromEffect.ConsumeStacks);
            Assert.Equal(hdDto.stack_from_effect.from, hitDamageStep.StackFromEffect.FromEffect);
            Assert.Equal(hdDto.scale_coef.scalar_operation.value, hitDamageStep.ScaleCoefficient.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, hitDamageStep.ScaleCoefficient.Operation.ModifierOperation);
            Assert.Equal(StatKind.MeleeDamage, hitDamageStep.ScaleCoefficient.Stat);
        }

        [Fact]
        public void CanMapDotDamageStepDto()
        {
            var dotDto = new DotDamageStepDto()
            {
                base_damage = 1,
                crit = true,
                damage_types = [DamageTypeDto.melee],
                duration = new PermanentDurationDto() { permanent = true },
                frequency = 1,
                stacking = new StackDefaultDto() { max_stacks = 1, refresh_mode = StackDefaultDto.RefreshMode.reset_time, stacks_per_application = 1 },
                timing = DotDamageStepDto.TimingKind.start_turn,
                scale_coef = new()
                {
                    scalar_operation = new(Dto.Common.Operations.ScalarOperationDto.Operation.mult, 1),
                    stat = StatDto.avoidance
                }
            };

            var dotDamageStep = dotDto.ToDomain();
            Assert.NotNull(dotDamageStep);
            Assert.Equal(dotDto.base_damage, dotDamageStep.BaseDamage);
            Assert.Equal(dotDto.crit, dotDamageStep.Crit);
            Assert.NotNull(dotDamageStep.DamageTypes);
            Assert.Single(dotDamageStep.DamageTypes);
            Assert.Equal(DamageType.Melee, dotDamageStep.DamageTypes.First());
            Assert.True(dotDamageStep.Duration.Type == GameData.src.Shared.Duration.Kind.Permanent);
            Assert.Equal(dotDto.frequency, dotDamageStep.Frequency);
            Assert.NotNull(dotDamageStep.Stacking);
            Assert.Equal(((StackDefaultDto)dotDto.stacking).max_stacks, ((StackDefault)dotDamageStep.Stacking).MaxStacks);
            Assert.Equal(StackRefreshMode.ResetTime, ((StackDefault)dotDamageStep.Stacking).RefreshMode);
            Assert.Equal(((StackDefaultDto)dotDto.stacking).stacks_per_application, ((StackDefault)dotDamageStep.Stacking).StacksPerApplication);
            Assert.Equal(DotDamageStep.TimingKind.StartTurn, dotDamageStep.Timing);
            Assert.Equal(dotDto.scale_coef.scalar_operation.value, dotDamageStep.ScaleCoefficient.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Mult, dotDamageStep.ScaleCoefficient.Operation.ModifierOperation);
            Assert.Equal(StatKind.Avoidance, dotDamageStep.ScaleCoefficient.Stat);

        }

        [Fact]
        public void CanMapSkillDto()
        {
            var skillDto = new SkillDto()
            {
                cooldown = 1,
                cost = 1,
                effects = [new HitDamageStepDto() {
                    base_damage =1 ,
                    crit = true,
                    damage_types = [DamageTypeDto.true_damage],
                    stack_from_effect = new StackFromEffectDto() {
                        consume_stacks = true,
                        from = "test_effect",
                    },
                    scale_coef = new() {
                        stat = StatDto.melee_damage,
                        scalar_operation = new (Dto.Common.Operations.ScalarOperationDto.Operation.add , 1)
                    }
                }],
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
            var hitDamageStep = skillDef.Effects.First() as HitDamageStep;
            Assert.NotNull(hitDamageStep);
            Assert.Equal(((HitDamageStepDto)skillDto.effects.First()).base_damage, hitDamageStep.BaseDamage);
            Assert.Equal(((HitDamageStepDto)skillDto.effects.First()).crit, hitDamageStep.Crit);
            Assert.NotNull(hitDamageStep.DamageTypes);
            Assert.Single(hitDamageStep.DamageTypes);
            Assert.Equal(DamageType.TrueDamage, hitDamageStep.DamageTypes.First());
            Assert.NotNull(hitDamageStep.StackFromEffect);
            Assert.Equal(1, hitDamageStep.ScaleCoefficient.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, hitDamageStep.ScaleCoefficient.Operation.ModifierOperation);
            Assert.Equal(StatKind.MeleeDamage, hitDamageStep.ScaleCoefficient.Stat);

            var firstEffect = skillDto.effects.FirstOrDefault() as HitDamageStepDto;
            Assert.NotNull(firstEffect);
            Assert.NotNull(firstEffect.stack_from_effect);
            Assert.Equal(firstEffect.stack_from_effect.consume_stacks, hitDamageStep.StackFromEffect.ConsumeStacks);
            Assert.Equal(firstEffect.stack_from_effect.from, hitDamageStep.StackFromEffect.FromEffect);
            Assert.Equal(skillDto.id, skillDef.Id);
            Assert.NotNull(skillDef.Presentation);
            Assert.Equal(skillDto.presentation.name, skillDef.Presentation.Name);
            Assert.Equal(skillDto.presentation.description, skillDef.Presentation.Description);
            Assert.NotNull(skillDef.ActivationRequirement);
            Assert.Equal(skillDto.activation_req.count, skillDef.ActivationRequirement.Count);
            Assert.Equal(ActivationRequirement.Kind.EffectPresent, skillDef.ActivationRequirement.ActivationKind);
        }
    }
}