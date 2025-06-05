using GameData.src.Effect.Stack;
using GameData.src.Effect.Talent;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill.SkillStep;
using GameData.src.Talent.TalentActions;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Dto.Effect;
using Infrastructure.Json.Dto.Talent;
using Infrastructure.Json.Mappers;
using Xunit;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class TalentMapperTests
    {
        [Fact]
        public void CanMapModifyHitActionDto()
        {
            var dto = new ModifyHitDamageActionDto()
            {
                skill = "test_skill",
                base_damage = new(Dto.Common.Operations.ScalarOperationDto.Operation.add, 1),
                crit = true,
                damage_types = new(Dto.Common.Operations.CollectionOperationDto.add, [DamageTypeDto.bleed, DamageTypeDto.burn])
            };

            var modifyHitAction = dto.ToDomain() as ModifyHitDamageAction;

            // Assert
            Assert.NotNull(modifyHitAction);
            Assert.Equal("test_skill", modifyHitAction.Skill);
            Assert.NotNull(modifyHitAction.BaseDamage);
            Assert.Equal(ScalarOperation.OperationKind.Add, modifyHitAction.BaseDamage.ModifierOperation);
            Assert.Equal(1, modifyHitAction.BaseDamage.Value);
            Assert.True(modifyHitAction.Crit);
            Assert.NotNull(modifyHitAction.DamageTypes);
            Assert.Contains(DamageType.Bleed, modifyHitAction.DamageTypes.Operation.Items);
            Assert.Contains(DamageType.Burn, modifyHitAction.DamageTypes.Operation.Items);
        }

        [Fact]
        public void CanMapModifyDotDamageDto()
        {
            var dto = new ModifyDotDamageActionDto()
            {
                base_damage = new(ScalarOperationDto.Operation.mult, 1),
                skill = "test_skill",
                crit = false,
                damage_types = new(CollectionOperationDto.add, [DamageTypeDto.elemental, DamageTypeDto.physical]),
                duration = new DurationOperationDto(new(ScalarOperationDto.Operation.mult, 1), null, null),
                frequency = new(ScalarOperationDto.Operation.add, 1),
                stacking = new StackDefaultDto() { max_stacks = 1, refresh_mode = StackDefaultDto.RefreshMode.add_time, stacks_per_application = 1 },
                timing = Dto.Skill.DotDamageStepDto.TimingKind.start_turn,
            };

            var modifyDotDamageAction = dto.ToDomain() as ModifyDotDamageAction;
            // Assert
            Assert.NotNull(modifyDotDamageAction);

            Assert.NotNull(modifyDotDamageAction.DamageTypes);
            Assert.Equal(CollectionOperationKind.Add, modifyDotDamageAction.DamageTypes.Operation.Operation);

            Assert.NotNull(modifyDotDamageAction.Timing);
            Assert.Equal(DotDamageStep.TimingKind.StartTurn, modifyDotDamageAction.Timing.Value);
            Assert.Equal(1, modifyDotDamageAction.Frequency?.Value);

            Assert.NotNull(modifyDotDamageAction.Duration);
            Assert.Equal(ScalarOperation.OperationKind.Mult, modifyDotDamageAction.Duration.Turns?.ModifierOperation);
            Assert.Equal(1, modifyDotDamageAction.Duration.Turns?.Value);

            Assert.NotNull(modifyDotDamageAction.Crit);
            Assert.False(modifyDotDamageAction.Crit);

            Assert.Equal(dto.skill, modifyDotDamageAction.SkillId);

            var stackDto = dto.stacking as StackDefaultDto;
            var stackDefault = modifyDotDamageAction.StackStrategy as StackDefault;

            Assert.NotNull(stackDto);
            Assert.NotNull(stackDefault);

            Assert.Equal(stackDto.max_stacks, stackDefault.MaxStacks);
            Assert.Equal(StackRefreshMode.AddTime, stackDefault.RefreshMode);
            Assert.Equal(stackDto.stacks_per_application, stackDefault.StacksPerApplication);


            Assert.NotNull(modifyDotDamageAction.DamageTypes);
            Assert.Collection(modifyDotDamageAction.DamageTypes.Operation.Items,
                item => Assert.Equal(DamageType.Elemental, item),
                item => Assert.Equal(DamageType.Physical, item));
        }

        [Fact]
        public void CanMapModifySkillActionDto()
        {
            var dto = new ModifySkillActionDto()
            {
                activation_req = new Dto.Skill.ActivationRequirementDto()
                {
                    count = 1,
                    type = Dto.Skill.ActivationRequirementDto.ActivationKind.hp_pct_below
                },
                cooldown = new(ScalarOperationDto.Operation.add, 1),
                cost = new(ScalarOperationDto.Operation.add, 1),
                skill = "skill_test",
            };

            var skillAction = dto.ToDomain() as ModifySkillAction;

            Assert.NotNull(skillAction);

            Assert.Equal(dto.cooldown.value, skillAction.Cooldown?.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, skillAction.Cooldown?.ModifierOperation);
            Assert.Equal(dto.cost.value, skillAction.Cost?.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, skillAction.Cost?.ModifierOperation);

            Assert.Equal(dto.activation_req.count, skillAction.ActivationRequirement?.Count);
            Assert.Equal(ActivationRequirement.Kind.HpBelowPercentage, skillAction.ActivationRequirement?.ActivationKind);

            Assert.Equal(dto.skill, skillAction.Skill);
        }

        [Fact]
        public void CanMapModifyEffectDto()
        {
            var dto = new ModifyEffectActionDto()
            {
                id = "test_effect",
                damage_types = new DamageTypeCollectionOperationDto(CollectionOperationDto.add, [DamageTypeDto.bleed]),
                duration = new DurationOperationDto(new(ScalarOperationDto.Operation.add, 1), null, null),
                leech = new ScalarOperationDto(ScalarOperationDto.Operation.add, 1),
                stacking = new StackFromEffectDto() { consume_stacks = true, from = "test_effect" }
            };

            var modifyEffect = dto.ToDomain() as ModifyEffectAction;

            Assert.NotNull(modifyEffect);
            Assert.Equal(dto.id, modifyEffect.Id);
            Assert.Equal(dto.damage_types.items.Length, modifyEffect.DamageTypes?.Operation?.Items.Count);
            Assert.NotNull(modifyEffect.DamageTypes);
            Assert.Collection(modifyEffect.DamageTypes.Operation.Items,
                item => Assert.Equal(DamageType.Bleed, item));

            Assert.NotNull(modifyEffect.Duration);
            Assert.Equal(ScalarOperation.OperationKind.Add, modifyEffect.Duration.Turns?.ModifierOperation);
            Assert.Equal(1, modifyEffect.Duration.Turns?.Value);

            Assert.NotNull(modifyEffect.Leech);
            Assert.Equal(ScalarOperation.OperationKind.Add, modifyEffect.Leech.ModifierOperation);
            Assert.Equal(1, modifyEffect.Leech.Value);

            var stackFromEffectDto = dto.stacking as StackFromEffectDto;
            var stackFromEffect = modifyEffect.Stacking as StackFromEffect;

            Assert.NotNull(stackFromEffectDto);
            Assert.NotNull(stackFromEffect);

            Assert.Equal(stackFromEffectDto.consume_stacks, stackFromEffect.ConsumeStacks);
            Assert.Equal(stackFromEffectDto.from, stackFromEffect.FromEffect);
        }

        [Fact]
        public void CanMapTalentDto()
        {
            var dto = new TalentDto()
            {
                actions = [
                    new ModifyEffectActionDto() { id = "test_effect", damage_types = new(CollectionOperationDto.add, [DamageTypeDto.elemental, DamageTypeDto.burn]), duration = new DurationOperationDto(null, true, null) },
                    new ModifyDotDamageActionDto() {skill = "test_skill", base_damage = new(ScalarOperationDto.Operation.add, 1), crit = true, damage_types = new(CollectionOperationDto.add, [DamageTypeDto.melee, DamageTypeDto.nature])}
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
            Assert.Collection(talent.Actions,
                action =>
                {
                    var modifyEffectAction = action as ModifyEffectAction;
                    Assert.NotNull(modifyEffectAction);
                    Assert.Equal("test_effect", modifyEffectAction.Id);
                    Assert.NotNull(modifyEffectAction.DamageTypes);
                    Assert.Collection(modifyEffectAction.DamageTypes.Operation.Items,
                        item => Assert.Equal(DamageType.Elemental, item),
                        item => Assert.Equal(DamageType.Burn, item));
                    Assert.NotNull(modifyEffectAction.Duration);
                    Assert.True(modifyEffectAction.Duration.Permanent);
                },
                action =>
                {
                    var modifyDotDamageAction = action as ModifyDotDamageAction;
                    Assert.NotNull(modifyDotDamageAction);
                    Assert.Equal("test_skill", modifyDotDamageAction.SkillId);
                    Assert.NotNull(modifyDotDamageAction.BaseDamage);
                    Assert.Equal(ScalarOperation.OperationKind.Add, modifyDotDamageAction.BaseDamage.ModifierOperation);
                    Assert.Equal(1, modifyDotDamageAction.BaseDamage.Value);
                    Assert.True(modifyDotDamageAction.Crit);
                    Assert.NotNull(modifyDotDamageAction.DamageTypes);
                    Assert.Collection(modifyDotDamageAction.DamageTypes.Operation.Items,
                        item => Assert.Equal(DamageType.Melee, item),
                        item => Assert.Equal(DamageType.Nature, item));
                });

            Assert.Equal(dto.id, talent.Id);
            Assert.NotNull(talent.Presentation);
            Assert.Equal(dto.presentation.description, talent.Presentation.Description);
            Assert.Equal(dto.presentation.name, talent.Presentation.Name);
        }
    }
}